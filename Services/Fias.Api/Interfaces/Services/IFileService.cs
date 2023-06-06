using Fias.Api.Models.File;
using Fias.Api.ViewModels.Models;

using Microsoft.AspNetCore.WebUtilities;

namespace Fias.Api.Interfaces.Services
{
    public interface IFileService
    {
        Task<FileViewModel> UploadFileAsync(MultipartReader reader, string directory);
    }
}
