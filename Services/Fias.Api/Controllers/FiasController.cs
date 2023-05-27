using Fias.Api.Interfaces.Services;
using Fias.Api.Models.Options.DataBase;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace Fias.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FiasController : ControllerBase
    {
        private readonly IFileService _fileUploadService;

        public FiasController(IFileService fileUploadService, IOptions<DbSettingsOption> dbOptions)
        {
            _fileUploadService = fileUploadService ?? throw new ArgumentNullException(nameof(fileUploadService));
        }

        [HttpPost]
        [Route("updateDataBaseFromFile")]
        public async Task<IActionResult> UpdateDbFromFile()
        {
            if (string.IsNullOrWhiteSpace(Request.ContentType))
                return BadRequest();
            var boundary = HeaderUtilities.RemoveQuotes(
                MediaTypeHeaderValue.Parse(Request.ContentType).Boundary
                ).Value;
            var reader = new MultipartReader(boundary, HttpContext.Request.Body);

            var originFileName = await _fileUploadService.UploadFileAsync(reader, Asp.GetTempPath());
            var d = originFileName.First();
            await _fileUploadService.InsertToDbFromUploadedFileAsync(d);

            return Ok();
        }

        [HttpPost]
        [Route("restoreDataBaseFromFile")]
        public async Task<IActionResult> RestoreDbFromFile()
        {
            if (string.IsNullOrWhiteSpace(Request.ContentType))
                return BadRequest();
            var boundary = HeaderUtilities.RemoveQuotes(
                MediaTypeHeaderValue.Parse(Request.ContentType).Boundary
                ).Value;
            var reader = new MultipartReader(boundary, HttpContext.Request.Body);
            //var fullName = Path.Combine(Asp.GetTempPath(), Path.GetRandomFileName());
            var originFileName = await _fileUploadService.UploadFileAsync(reader, Asp.GetTempPath());

            //await _fileUploadService.InsertToDbFromArchiveFileAsync(fullName, true);

            return Ok();
        }
    }
}
