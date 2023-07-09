using System.Text.RegularExpressions;
using System.Xml.Serialization;

using AutoMapper;

using Fias.Api.Enums;
using Fias.Api.Interfaces.Services;
using Fias.Api.Interfaces.XmlModels;
using Fias.Api.Models.File;
using Fias.Api.Entities;
using Fias.Api.Models.FiasModels.XmlModels.Houses;
using Fias.Api.Models.FiasModels.XmlModels.ParamTypes;
using Fias.Api.Models.FiasModels.XmlModels.HousesParams;
using System.Xml;
using Fias.Api.Contexts;
using System.Reflection;
using Fias.Api.Extensions;
using Fias.Api.Models.FiasModels.XmlModels.AddrObj;
using Fias.Api.Models.FiasModels.XmlModels.AddrObjParams;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;
using Fias.Api.Exceptions;

namespace Fias.Api.Services
{
    public class XmlService : IXmlService
    {
        private readonly IMapper _mapper;
        private readonly IDbContextFactory<AppDbContext> _contextFactory;
        private readonly ILogger<XmlService> _loger;
        private string _regionCode;

        public XmlService(
            IMapper mapper,
            ILogger<XmlService> loger,
            IDbContextFactory<AppDbContext> contextFactory)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
            _loger = loger ?? throw new ArgumentNullException(nameof(loger));
        }

        public T? DeserializeFiasXml<T>(FileStream xmlFile) where T : class, IXmlModel
        {
            var serializer = new XmlSerializer(typeof(T));
            var theObject = serializer.Deserialize(xmlFile) as T;
            return theObject;
        }

        public T? DeserializeFiasXml<T>(string xml) where T : class, IXmlModel
        {
            var ser = new XmlSerializer(typeof(T));

            using (StringReader sr = new StringReader(xml))
            {
                return ser.Deserialize(sr) as T;
            }
        }

        public XmlModelType GetXmlModelTypeFromXmlFile(string xmlFileName)
        {
            var fileName = GetXmlKeyFromXmlFileName(xmlFileName);
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new NotImplementedException();
            }

            return fileName.ToEnum(XmlModelType.Unknown);
        }

        private async Task RemoveAllXmlTableAsync(AppDbContext context)
        {
            _loger.LogInformation($"delete all from xml tables: START");
            var query = $"REGIONCODE = '{_regionCode}'";
            await context.DeleteFromTableWithExpressionAsync<HouseParamEntity>(query);
            await context.DeleteFromTableWithExpressionAsync<ParamTypesEntity>();
            await context.DeleteFromTableWithExpressionAsync<AddrObjEntity>(query);
            await context.DeleteFromTableWithExpressionAsync<AddrObjParamEntity>(query);
            await context.SaveChangesAsync();

            _loger.LogInformation($"delete all from xml tables: FINISH");
        }

        private async Task InsertToDbFromXmlFileAsync(DbContext context, TempFile tempXml, bool isRestoreDb = false)
        {
            if (string.IsNullOrWhiteSpace(tempXml.FullFilePath))
                return;
            var xmlModelType = GetXmlModelTypeFromXmlFile(tempXml.OriginFileName);

            using (var reader = XmlReader.Create(tempXml.FullFilePath, new XmlReaderSettings() { Async = true }))
            {
                switch (xmlModelType)
                {
                    case XmlModelType.AS_HOUSES:
                        {
                            await ReadXmlWriteDbAsync<HouseModel, HouseEntity>(context, reader, isRestoreDb);
                            break;
                        }
                    case XmlModelType.AS_HOUSES_PARAMS:
                        {
                            await ReadXmlWriteDbAsync<HouseParamModel, HouseParamEntity>(context, reader, isRestoreDb);
                            break;
                        }

                    case XmlModelType.AS_PARAM_TYPES:
                        {
                            await ReadXmlWriteDbAsync<ParamTypesModel, ParamTypesEntity>(context, reader, isRestoreDb);
                            break;
                        }
                    case XmlModelType.AS_ADDR_OBJ:
                        {
                            await ReadXmlWriteDbAsync<AddrObjModel, AddrObjEntity>(context, reader, isRestoreDb);
                            break;
                        }
                    case XmlModelType.AS_ADDR_OBJ_PARAMS:
                        {
                            await ReadXmlWriteDbAsync<AddrObjParamModel, AddrObjParamEntity>(context, reader, isRestoreDb);
                            break;
                        }
                }
            }
        }

        public async Task InsertToDbFromArchiveAsync(TempFile uploadFile, IEnumerable<string> selectdeRegions, bool isRestoreDb = false)
        {
            var fileExtencion = Path.GetExtension(uploadFile.OriginFileName)?.ToLower();
            if (fileExtencion == ".zip")
            {
                using (ZipArchive archive = ZipFile.OpenRead(uploadFile.FullFilePath))
                {
                    var groups = archive.Entries
                        .GroupBy(x => x.FullName.Split('/').First());
                    var hzFiles = groups
                        .Where(x => !Regex.IsMatch(x.Key.Trim(), @"^\d{2}", RegexOptions.IgnoreCase))
                        .SelectMany(x => x)
                        .ToList();
                    var regions = groups
                        .AsParallel()
                        .Where(x =>
                        {
                            var key = x.Key.Trim();
                            return Regex.IsMatch(key, @"^\d{2}", RegexOptions.IgnoreCase)
                                ? selectdeRegions is null || !selectdeRegions.Any() 
                                    ? true 
                                    : selectdeRegions.Any(y => y.StartsWith(key))
                                : false;
                        }).ToList();

                    var iterator = 0;
                    foreach (var group in regions)
                    {
                        using (var context = await _contextFactory.CreateDbContextAsync())
                        {
                            using (var transaction = await context.Database.BeginTransactionAsync())
                            {
                                try
                                {
                                    _loger.LogInformation($"Work with region {group.Key}: START");
                                    _regionCode = group.Key;
                                    if (isRestoreDb)
                                    {
                                        await RemoveAllXmlTableAsync(context);
                                    }
                                    if (iterator == 0)
                                    {
                                        foreach (ZipArchiveEntry hz in hzFiles)
                                        {
                                            await GetFileFromArchiveAsync(context, hz, uploadFile.FullFilePath, isRestoreDb);
                                        }
                                    }
                                    iterator++;

                                    foreach (ZipArchiveEntry entry in group)
                                    {
                                        await GetFileFromArchiveAsync(context, entry, uploadFile.FullFilePath, isRestoreDb);
                                    }

                                    await transaction.CommitAsync();
                                }
                                catch (Exception ex)
                                {
                                    await transaction.RollbackAsync();
                                    _loger.LogError(ex.ToString());
                                }
                            }
                                

                            _loger.LogInformation($"Work with region {group.Key}: FINISH");
                        }
                        
                    }
                }
            }            
            else
            {
                throw new UserException("Unsupported file format.");
            }
        }

        private async Task GetFileFromArchiveAsync(DbContext context, ZipArchiveEntry entry, string fullFilePath, bool isRestoreDb = false)
        {
            // Gets the full path to ensure that relative segments are removed.
            var destinationPath = Path.Combine(
                $"{Path.GetDirectoryName(fullFilePath)}\\{Path.GetFileNameWithoutExtension(fullFilePath)}",
                Path.GetRandomFileName());
            var directory = Path.GetDirectoryName(destinationPath);
            if (!Directory.Exists(directory) && !string.IsNullOrWhiteSpace(directory))
                Directory.CreateDirectory(directory);

            if (entry.FullName.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
            {
                // Ordinal match is safest, case-sensitive volumes can be mounted within volumes that
                // are case-insensitive.
                if (destinationPath.StartsWith(destinationPath, StringComparison.Ordinal))
                {
                    entry.ExtractToFile(destinationPath);
                    await InsertToDbFromXmlFileAsync(context, new TempFile(destinationPath, entry.Name, entry.Length), isRestoreDb);
                }
            }
            else if (entry.FullName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
            {

            }

            File.Delete(destinationPath);
        }

        private string? GetXmlKeyFromXmlFileName(string fileName)
        {
            var regex = new Regex("^([^0-9]+)");
            var matches = regex.Matches(fileName);
            return matches.FirstOrDefault()?.Value?.TrimEnd('_');
        }

        private string? GetXmlRoot<T>() where T: class, IXmlRowModel
        {
            var type = typeof(T);
            XmlRootAttribute? atribut = type.GetCustomAttribute(typeof(XmlRootAttribute), false) as XmlRootAttribute;
            return atribut?.ElementName;
        }

        private T GetHouseModelFromReader<T>(XmlReader reader) where T: class, IXmlRowModel
        {
            var type = typeof(T);
            var ob = Activator.CreateInstance(type) as T;
            var properties = ob.GetType().GetProperties();
            foreach (var property in properties)
            {
                if (property.GetCustomAttribute(typeof(XmlAttributeAttribute), false) is not XmlAttributeAttribute pr)
                    continue;
                var atributeName = string.IsNullOrWhiteSpace(pr.AttributeName)
                    ? nameof(property.Name) 
                    : pr.AttributeName;
                var value = reader.GetAttribute(atributeName);
                if (string.IsNullOrWhiteSpace(value))
                    continue;
                property.SetValueType(ob, value);
            }

            return ob;
        }

        private async Task ReadXmlWriteDbAsync<TModel, TEntity>(DbContext context, XmlReader reader, bool isRestoreDb = false)
            where TModel : class, IXmlRowModel
            where TEntity : BaseEntity
        {
            var xmlRoot = GetXmlRoot<TModel>();
            var iterator = 0;

            var buffer = new List<TEntity>(50);
            await context.SetIdentityInsertAsync<TEntity>(true);
            try
            {
                while (await reader.ReadAsync())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            {
                                if (reader.LocalName == xmlRoot)
                                {
                                    var model = GetHouseModelFromReader<TModel>(reader);
                                    var entity = _mapper.Map<TEntity>(model);
                                    if (entity is BaseRegionEntity)
                                    {
                                        var entityRegion = entity as BaseRegionEntity;
                                        entityRegion.RegionCode = _regionCode;
                                    }

                                    if (isRestoreDb)
                                    {
                                        if (iterator < 50)
                                        {
                                            buffer.Add(entity);
                                            iterator++;
                                        }
                                        else
                                        {
                                            await context.BulkInsertAsync(buffer, true);
                                            buffer.Clear();
                                            iterator = 0;
                                        }
                                    }
                                    break;
                                }

                                break;
                            }
                    }
                }

                if (isRestoreDb && buffer.Any())
                {
                    await context.BulkInsertAsync(buffer, true);
                }
            }
            finally
            {
                await context.SetIdentityInsertAsync<TEntity>(false);
            }
        }
    }
}
