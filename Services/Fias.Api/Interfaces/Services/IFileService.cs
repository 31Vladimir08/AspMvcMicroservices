using Fias.Api.Enums;
using Fias.Api.Models.File;
using Microsoft.AspNetCore.WebUtilities;

namespace Fias.Api.Interfaces.Services
{
    public interface IFileService
    {
        Task<List<TempFile>> UploadFileAsync(MultipartReader reader, string directory);
        Task InsertToDbFromUploadedFileAsync(TempFile uploadFile, bool isRestoreDb = false);
        Task InsertToDbFromXmlFileAsync(TempFile uploadFile, bool isRestoreDb = false);
    }
}
