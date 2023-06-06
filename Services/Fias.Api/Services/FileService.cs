using System.Text.Json;

using Fias.Api.Interfaces.Services;
using Fias.Api.Models.File;
using Fias.Api.ViewModels.Filters;
using Fias.Api.ViewModels.Models;

using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;

namespace Fias.Api.Services
{
    public class FileService : IFileService
    {
        private readonly ILogger<FileService> _loger;

        public FileService(
            ILogger<FileService> loger) 
        {
            _loger = loger ?? throw new ArgumentNullException(nameof(loger));
        }

        public async Task<FileViewModel> UploadFileAsync(MultipartReader reader, string directory)
        {
            var fileVm = new FileViewModel();
            if (reader is null || string.IsNullOrWhiteSpace(directory))
                return fileVm;
            MultipartSection? section;
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

                            fileVm.TempFiles.Add(new TempFile(fullName, contentDisposition.FileName.Value));
                        }
                        else if (contentDisposition is not null 
                            && contentDisposition.DispositionType.Equals("form-data")
                            && contentDisposition.Name == "filter")
                        {
                            var fileSection = section.AsFormDataSection();
                            if (fileSection is null)
                                continue;
                            var json = await fileSection.GetValueAsync();
                            var selectedRegions = JsonSerializer.Deserialize<RegionsFilterViewModel>(json)?.SelectedRegions;
                            if (selectedRegions != null && selectedRegions.Any())
                                fileVm.SelectedRegions = selectedRegions;
                        }
                    }
                }

                return fileVm;
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
