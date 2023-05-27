using System.IO.Compression;

using AutoMapper;

using Fias.Api.Entities;
using Fias.Api.Enums;
using Fias.Api.Interfaces;
using Fias.Api.Interfaces.Entities;
using Fias.Api.Interfaces.Services;
using Fias.Api.Models.FiasModels.XmlModels.AddrObj;
using Fias.Api.Models.FiasModels.XmlModels.Houses;
using Fias.Api.Models.FiasModels.XmlModels.HousesParams;
using Fias.Api.Models.FiasModels.XmlModels.ParamTypes;
using Fias.Api.Models.File;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;

namespace Fias.Api.Services
{
    public class FileService : IFileService
    {
        private readonly IXmlService _xmlService;
        private readonly IMapper _mapper;
        private readonly IDbContext _dbContext;

        public FileService(
            IXmlService xmlService,
            IMapper mapper,
            IDbContext dbContext) 
        {
            _xmlService = xmlService ?? throw new ArgumentNullException(nameof(xmlService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task InsertToDbFromUploadedFileAsync(TempFile uploadFile, bool isRestoreDb = false)
        {
            using (ZipArchive archive = ZipFile.OpenRead(uploadFile.FullFilePath))
            {
                var i = 0;
                await _dbContext.Database.BeginTransactionAsync();
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
                            //здесь после того как файл распакован, десериализовать и записать в базу
                            
                            await InsertToDbFromXmlFileAsync(new TempFile(destinationPath, entry.Name), isRestoreDb);
                        }
                    }
                    else if (entry.FullName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                    {

                    }

                    File.Delete(destinationPath);
                    i++;
                }

                await _dbContext.Database.CommitTransactionAsync();
            }
        }

        public async Task InsertToDbFromXmlFileAsync(TempFile tempXml, bool isRestoreDb = false)
        {
            if (string.IsNullOrWhiteSpace(tempXml.FullFilePath))
                return;
            var xmlModelType = _xmlService.GetXmlModelTypeFromXmlFile(tempXml.OriginFileName);
            using (var file = new FileStream(tempXml.FullFilePath, FileMode.Open, FileAccess.Read))
            {
                switch (xmlModelType)
                {
                    case XmlModelType.Houses:
                        {
                            var model = _xmlService.DeserializeFiasXml<HOUSES>(file);
                            var entities = model?.HOUSE.AsParallel().Select(_mapper.Map<HouseEntity>).ToList();
                            await InsertUpdateEntities(entities, isRestoreDb);
                            break;
                        }
                    case XmlModelType.HousesParams:
                        {
                            var model = _xmlService.DeserializeFiasXml<PARAMS>(file);
                            var entities = model?.PARAM.AsParallel().Select(_mapper.Map<HouseParamsEntity>).ToList();
                            await InsertUpdateEntities(entities, isRestoreDb);
                            break;
                        }                      
                        
                    case XmlModelType.ParamTypes:
                        {
                            var model = _xmlService.DeserializeFiasXml<PARAMTYPES>(file);
                            var entities = model?.PARAMTYPE.AsParallel().Select(_mapper.Map<ParamTypesEntity>).ToList();
                            await InsertUpdateEntities(entities, isRestoreDb);
                            break;
                        }
                    case XmlModelType.AddrObj:
                        {
                            var model = _xmlService.DeserializeFiasXml<ADDRESSOBJECTS>(file);
                            var entities = model?.OBJECT.AsParallel().Select(_mapper.Map<AddrObjEntity>).ToList();
                            await InsertUpdateEntities(entities, isRestoreDb);
                            break;
                        }
                    case XmlModelType.AddrObjParams:
                        {
                            var model = _xmlService.DeserializeFiasXml<Models.FiasModels.XmlModels.AddrObjParams.PARAMS>(file);
                            var entities = model?.PARAM.AsParallel().Select(_mapper.Map<AddrObjParamEntity>).ToList();
                            await InsertUpdateEntities(entities, isRestoreDb);
                            break;
                        }
                }
            }
        }

        public async Task<List<TempFile>> UploadFileAsync(MultipartReader reader, string directory)
        {
            var filesNames = new List<TempFile>();
            if (reader is null || string.IsNullOrWhiteSpace(directory))
                return filesNames;
            MultipartSection section;
            
            while ((section = await reader.ReadNextSectionAsync()) is not null)
            {
                var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(
                    section.ContentDisposition, out var contentDisposition
                );
                if (hasContentDispositionHeader)
                {
                    if (!string.IsNullOrEmpty(contentDisposition?.FileName.Value) 
                        && contentDisposition.DispositionType.Equals("form-data"))
                    {
                        var fileSection = section.AsFileSection();
                        if (fileSection is null || fileSection.FileStream is null)
                            continue;
                        var bufferSize = 32 * 1024;
                        var buffer = new byte[bufferSize];
                        var fullName = Path.Combine(directory, Path.GetRandomFileName());
                        using (var fstream = new FileStream(fullName, FileMode.Create, FileAccess.Write))
                        {
                            while (true)
                            {
                                var bytesRead = await fileSection.FileStream.ReadAsync(buffer.AsMemory(0, bufferSize));
                                await fstream.WriteAsync(buffer, 0, bytesRead);
                                if (bytesRead == 0) 
                                    break;
                            }
                        }

                        filesNames.Add(new TempFile(fullName, contentDisposition.FileName.Value));
                    }
                }
            }

            return filesNames;
        }

        private async Task InsertUpdateEntities<TEntity>(List<TEntity>? entities, bool isRestoreDb = false) where TEntity : class, IEntity
        {
            if (entities is null || entities.Count == 0) 
                return;
            if (isRestoreDb)
            {
                _dbContext.Set<IEntity>().RemoveRange(_dbContext.Set<IEntity>());
                await _dbContext.SaveChangesAsync();
                _dbContext.Set<IEntity>().AddRange(entities);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                foreach (var entity in entities)
                {
                    if (await _dbContext.Set<TEntity>()
                        .AsNoTracking().AnyAsync(q => q.Id == entity.Id))
                    {
                        _dbContext.Set<IEntity>().Update(entity);
                        await _dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        _dbContext.Set<IEntity>().Add(entity);
                        await _dbContext.SaveChangesAsync();
                    }
                }
            }            
        }
    }
}
