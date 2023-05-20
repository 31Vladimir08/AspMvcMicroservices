using System.IO;
using System.IO.Compression;

using Fias.Api.Interfaces.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;

namespace Fias.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FiasController : ControllerBase
    {
        private readonly IFileService _streamFileUploadService;

        public FiasController(IFileService streamFileUploadService)
        {
            _streamFileUploadService = streamFileUploadService;
        }

        [HttpPost]
        [Route("uploadFile")]
        public async Task<IActionResult> UploadPhysical()
        {
            if (string.IsNullOrWhiteSpace(Request.ContentType))
                return BadRequest();
            var boundary = HeaderUtilities.RemoveQuotes(
                MediaTypeHeaderValue.Parse(Request.ContentType).Boundary
                ).Value;
            var reader = new MultipartReader(boundary, HttpContext.Request.Body);

            var filePath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "UploadedTempFiles"));
            Directory.CreateDirectory(filePath);
            var file = await _streamFileUploadService.UploadFile(reader, filePath);
            using (ZipArchive archive = ZipFile.OpenRead($"{filePath}\\Test.zip"))
            {
                var i = 0;
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    if (entry.FullName.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
                    {
                        // Gets the full path to ensure that relative segments are removed.
                        string destinationPath = Path.GetFullPath(Path.Combine($"{filePath}\\Test", entry.FullName));
                        var directory = Path.GetDirectoryName(destinationPath);
                        if (!Directory.Exists(directory))
                            Directory.CreateDirectory(directory);

                        // Ordinal match is safest, case-sensitive volumes can be mounted within volumes that
                        // are case-insensitive.
                        if (destinationPath.StartsWith(destinationPath, StringComparison.Ordinal))
                            entry.ExtractToFile(destinationPath);
                    }
                    i++;
                }
            }
            //ZipFile.ExtractToDirectory($"{filePath}\\Test.zip", $"{filePath}\\Test");
            FileInfo fileInf = new FileInfo($"{filePath}\\{file.fileName}");
            if (fileInf.Exists)
            {
                fileInf.Delete();
            }
            return Ok();
        }
    }
}
