﻿using System.Text.RegularExpressions;
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
using Fias.Api.Exceptions;
using Fias.Api.Extensions;
using Fias.Api.Models.FiasModels.XmlModels.AddrObj;
using Fias.Api.Models.FiasModels.XmlModels.AddrObjParams;
using Fias.Api.Models.Options.DataBase;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;

namespace Fias.Api.Services
{
    public class XmlService : IXmlService
    {
        private Dictionary <string, XmlModelType> _attributes;
        private readonly IMapper _mapper;
        private readonly IDbContextFactory<AppDbContext> _contextFactory;
        //private readonly AppDbContext _dbContext;
        private readonly ILogger<XmlService> _loger;
        private readonly int _bufferDb;

        public XmlService(
            IMapper mapper,
            ILogger<XmlService> loger,
            IDbContextFactory<AppDbContext> contextFactory,
            IOptions<DbSettingsOption> dbOptions)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
            _loger = loger ?? throw new ArgumentNullException(nameof(loger));
            _bufferDb = dbOptions.Value.Buffer;
            _attributes = GetXmlDictionary();
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

            var isTypeExist =_attributes.TryGetValue(fileName, out var type);
            return isTypeExist ? type : XmlModelType.Unknown;
        }

        private async Task RemoveAllXmlTableAsync(AppDbContext dbContext)
        {
            _loger.LogInformation($"delete all from xml table: START");
            await dbContext.Set<HouseEntity>().ExecuteDeleteAsync();
            await dbContext.Set<HouseParamEntity>().ExecuteDeleteAsync();
            await dbContext.Set<ParamTypesEntity>().ExecuteDeleteAsync();
            await dbContext.Set<AddrObjEntity>().ExecuteDeleteAsync();
            await dbContext.Set<AddrObjParamEntity>().ExecuteDeleteAsync();
            await dbContext.SaveChangesAsync();
            _loger.LogInformation($"delete all from xml table: FINISH");
        }

        private async Task InsertToDbFromXmlFileAsync(AppDbContext context, TempFile tempXml, bool isRestoreDb = false)
        {
            if (string.IsNullOrWhiteSpace(tempXml.FullFilePath))
                return;
            var xmlModelType = GetXmlModelTypeFromXmlFile(tempXml.OriginFileName);

            using (var reader = XmlReader.Create(tempXml.FullFilePath, new XmlReaderSettings() { Async = true }))
            {
                switch (xmlModelType)
                {
                    case XmlModelType.Houses:
                        {
                            await ReadXmlWriteDbAsync<HouseModel, HouseEntity>(context, reader, isRestoreDb);
                            break;
                        }
                    case XmlModelType.HousesParams:
                        {
                            await ReadXmlWriteDbAsync<HouseParamModel, HouseParamEntity>(context, reader, isRestoreDb);
                            break;
                        }

                    case XmlModelType.ParamTypes:
                        {
                            await ReadXmlWriteDbAsync<ParamTypesModel, ParamTypesEntity>(context, reader, isRestoreDb);
                            break;
                        }
                    case XmlModelType.AddrObj:
                        {
                            await ReadXmlWriteDbAsync<AddrObjModel, AddrObjEntity>(context, reader, isRestoreDb);
                            break;
                        }
                    case XmlModelType.AddrObjParams:
                        {
                            await ReadXmlWriteDbAsync<AddrObjParamModel, AddrObjParamEntity>(context, reader, isRestoreDb);
                            break;
                        }
                }
            }
        }

        public async Task InsertToDbFromArchiveAsync(TempFile uploadFile, bool isRestoreDb = false)
        {
            using (var context = await _contextFactory.CreateDbContextAsync())
            {
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        if (isRestoreDb)
                        {
                            await RemoveAllXmlTableAsync(context);
                        }

                        var fileExtencion = Path.GetExtension(uploadFile.OriginFileName)?.ToLower();
                        if (fileExtencion == ".xml")
                        {
                            await InsertToDbFromXmlFileAsync(context, uploadFile, isRestoreDb);
                        }
                        else if (fileExtencion == ".zip")
                        {
                            using (ZipArchive archive = ZipFile.OpenRead(uploadFile.FullFilePath))
                            {
                                foreach (ZipArchiveEntry entry in archive.Entries)
                                {
                                    // Gets the full path to ensure that relative segments are removed.
                                    var destinationPath = Path.Combine(
                                        $"{Path.GetDirectoryName(uploadFile.FullFilePath)}\\{Path.GetFileNameWithoutExtension(uploadFile.FullFilePath)}",
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
                            }
                        }
                        else if (fileExtencion == ".txt")
                        {

                        }
                        else
                        {

                        }

                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
        }

        private async Task InsertOrUpdateAsync<TEntity>(AppDbContext context, TEntity? entity, bool isRestoreDb = false) where TEntity : BaseEntity
        {
            if (isRestoreDb)
            {
                if (entity is null)
                {
                    return;
                }

                await context.Set<TEntity>().AddAsync(entity);
            }
            else
            {
                if (entity is null)
                {
                    return;
                }

                var element = await context.Set<TEntity>().FirstOrDefaultAsync(q => q.Id == entity.Id);
                if (element is null)
                {
                    context.Set<TEntity>().Add(entity);
                }
                else
                {
                    entity.PkId = entity.PkId;
                    context.Set<TEntity>().Update(entity);
                }
            }
        }

        private Dictionary<string, XmlModelType> GetXmlDictionary()
        {
            return new Dictionary<string, XmlModelType>()
            {
                { "AS_ADDR_OBJ", XmlModelType.AddrObj },
                { "AS_ADDR_OBJ_PARAMS", XmlModelType.AddrObjParams },
                { "AS_HOUSES", XmlModelType.Houses },
                { "AS_HOUSES_PARAMS", XmlModelType.HousesParams },
                { "AS_PARAM_TYPES", XmlModelType.ParamTypes },
            };
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
            var ob = Activator.CreateInstance(type) as T ?? throw new UserException("БАНАНА");
            var properties = ob.GetType().GetProperties();
            foreach (var property in properties)
            {
                if (property.GetCustomAttribute(typeof(XmlAttributeAttribute), false) is not XmlAttributeAttribute pr)
                    continue;
                var value = reader.GetAttribute(pr.AttributeName);
                if (string.IsNullOrWhiteSpace(value))
                    continue;
                property.SetValueType(ob, value);
            }

            return ob;
        }

        private async Task ReadXmlWriteDbAsync<TModel, TEntity>(AppDbContext context, XmlReader reader, bool isRestoreDb = false)
            where TModel : class, IXmlRowModel
            where TEntity : BaseEntity
        {
            var xmlRoot = GetXmlRoot<TModel>();
            _loger.LogInformation($"Update db Table: START");
            var iterator = 1;
            context.ChangeTracker.AutoDetectChangesEnabled = false;
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
                                    await InsertOrUpdateAsync(context,entity, isRestoreDb);
                                    if (iterator % _bufferDb == 0)
                                    {
                                        await context.SaveChangesAsync();
                                    }

                                    iterator++;
                                    break;
                                }

                                break;
                            }
                    }
                }
                await context.SaveChangesAsync();
                _loger.LogInformation($"Update db Table: FINISH");
            }
            finally
            {
                context.ChangeTracker.AutoDetectChangesEnabled = true;
            }
        }
    }
}
