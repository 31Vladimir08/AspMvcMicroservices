using Microsoft.AspNetCore.WebUtilities;

namespace Fias.Api.Interfaces.Services
{
    public interface IFileService
    {
        Task<(string fileName, bool isUploadFile)> UploadFile(MultipartReader reader, string filePath);
    }
}
