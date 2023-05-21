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
        private readonly IXmlService _xmlService;

        public FiasController(IFileService streamFileUploadService, IXmlService xmlService)
        {
            _streamFileUploadService = streamFileUploadService;
            _xmlService = xmlService;
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
            var file = await _streamFileUploadService.UploadFileAsync(reader, filePath);
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
                        {
                            entry.ExtractToFile(destinationPath);
                            //здесь после того как файл распакован, десериализовать и записать в базу
                            /*using (var file = new FileStream("C:\\Users\\Admin\\Desktop\\gar_xml\\AS_HOUSES_20230518_7f62269b-64e6-4773-8875-18e7720eb162.xml", FileMode.Open, FileAccess.Read))
                            {
                                var res = _xmlService.DeserializeFiasXml<HOUSES>(file);
                                return Ok(res);
                            }*/
                        }
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
