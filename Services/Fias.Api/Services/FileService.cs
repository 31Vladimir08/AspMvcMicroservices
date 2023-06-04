using System.IO.Compression;

using Fias.Api.Contexts;
using Fias.Api.Interfaces.Services;
using Fias.Api.Models.File;

using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;

namespace Fias.Api.Services
{
    public class FileService : IFileService
    {
        private readonly IXmlService _xmlService;
        private readonly AppDbContext _dbContext;
        private readonly ILogger<FileService> _loger;

        public FileService(
            IXmlService xmlService,
            AppDbContext dbContext,
            ILogger<FileService> loger) 
        {
            _xmlService = xmlService ?? throw new ArgumentNullException(nameof(xmlService));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _loger = loger ?? throw new ArgumentNullException(nameof(loger));
        }

        //public async Task InsertToDbFromUploadedFileAsync(TempFile uploadFile, bool isRestoreDb = false)
        //{
        //    using (var transaction = await _dbContext.Database.BeginTransactionAsync())
        //    {
        //        try
        //        {
        //            if (isRestoreDb)
        //            {
        //                await _xmlService.RemoveAllXmlTableAsync();
        //            }

        //            var fileExtencion = Path.GetExtension(uploadFile.OriginFileName)?.ToLower();
        //            if (fileExtencion == ".xml")
        //            {
        //                await _xmlService.InsertToDbFromXmlFileAsync(uploadFile, isRestoreDb);
        //            }
        //            else if (fileExtencion == ".zip")
        //            {
        //                using (ZipArchive archive = ZipFile.OpenRead(uploadFile.FullFilePath))
        //                {
        //                    foreach (ZipArchiveEntry entry in archive.Entries)
        //                    {
        //                        // Gets the full path to ensure that relative segments are removed.
        //                        var destinationPath = Path.Combine(
        //                            $"{Path.GetDirectoryName(uploadFile.FullFilePath)}\\{Path.GetFileNameWithoutExtension(uploadFile.FullFilePath)}",
        //                            Path.GetRandomFileName());
        //                        var directory = Path.GetDirectoryName(destinationPath);
        //                        if (!Directory.Exists(directory) && !string.IsNullOrWhiteSpace(directory))
        //                            Directory.CreateDirectory(directory);

        //                        if (entry.FullName.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
        //                        {
        //                            // Ordinal match is safest, case-sensitive volumes can be mounted within volumes that
        //                            // are case-insensitive.
        //                            if (destinationPath.StartsWith(destinationPath, StringComparison.Ordinal))
        //                            {
        //                                entry.ExtractToFile(destinationPath);
        //                                await _xmlService.InsertToDbFromXmlFileAsync(new TempFile(destinationPath, entry.Name, entry.Length), isRestoreDb);
        //                            }
        //                        }
        //                        else if (entry.FullName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
        //                        {

        //                        }

        //                        File.Delete(destinationPath);
        //                    }
        //                }
        //            }
        //            else if (fileExtencion == ".txt")
        //            {

        //            }
        //            else
        //            {

        //            }

        //            await transaction.CommitAsync();
        //        }
        //        catch (Exception ex)
        //        {
        //            await transaction.RollbackAsync();
        //            throw;
        //        }
        //    }

            
        //}

        public async Task<List<TempFile>> UploadFileAsync(MultipartReader reader, string directory)
        {
            var filesNames = new List<TempFile>();
            if (reader is null || string.IsNullOrWhiteSpace(directory))
                return filesNames;
            MultipartSection section;
            try
            {
                Directory.CreateDirectory(directory);
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
                            var bufferSize = 4096;
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
            catch (Exception ex)
            {
                _loger.LogError($"{ex.Message}; {ex.StackTrace}; {ex.InnerException};");
                if (Directory.Exists(directory))
                {
                    Directory.Delete(directory, true);
                }
                throw;
            }
        }
    }
}
