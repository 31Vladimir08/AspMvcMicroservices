using System.IO.Compression;

using Fias.Api.Interfaces.Services;

using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;

namespace Fias.Api.Services
{
    public class FileService : IFileService
    {
        public async Task<(string fileName, bool isUploadFile)> UploadFileAsync(MultipartReader reader, string filePath)
        {
            if (reader is null)
                return ("Test.zip", false);
            MultipartSection section;
            
            while ((section = await reader.ReadNextSectionAsync()) is not null)
            {
                var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(
                    section.ContentDisposition, out var contentDisposition
                );
                if (hasContentDispositionHeader)
                {
                    if (contentDisposition is not null && contentDisposition.DispositionType.Equals("form-data") &&
                    (!string.IsNullOrEmpty(contentDisposition.FileName.Value)))
                    {
                        var fileSection = section.AsFileSection();
                        var bufferSize = 64 * 1024;
                        var buffer = new byte[bufferSize];

                        using (var fstream = new FileStream(Path.Combine(filePath, "Test.zip"), FileMode.Create, FileAccess.Write))
                        {
                            while (true)
                            {
                                var bytesRead = await fileSection.FileStream.ReadAsync(buffer, 0, bufferSize);
                                await fstream.WriteAsync(buffer, 0, bytesRead);
                                if (bytesRead == 0) 
                                    break;
                            }
                        }
                    }
                }
            }

            return ("Test.zip", true);


            /*while ((section = await reader.ReadNextSectionAsync()) != null)
            {
                var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(
                    section.ContentDisposition, out var contentDisposition
                );
                var contentDispo = section.GetContentDispositionHeader();

                if (contentDispo is not null && contentDispo.IsFileDisposition())
                {
                    var fileSection = section.AsFileSection();
                    var bufferSize = 32 * 1024;
                    var buffer = new byte[bufferSize];

                    int bytesRead;
                    using (var fstream = new FileStream(Path.Combine(filePath, contentDisposition.FileName.Value), FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        do
                        {
                            bytesRead = await fileSection.FileStream.ReadAsync(buffer, 0, bufferSize);
                            await fstream.WriteAsync(buffer, 0, bytesRead);
                        } while (bytesRead > 0);
                    }          
                }
                else if (contentDispo is not null && contentDispo.IsFormDisposition())
                {
                    var formSection = section.AsFormDataSection();
                    var value = await formSection.GetValueAsync();
                    valuesByKey.Add(formSection.Name, value);
                }
            }

            return true;*/
        }
    }
}
