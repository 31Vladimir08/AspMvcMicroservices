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
        private readonly IFileService _fileUploadService;

        public FiasController(
            IFileService fileUploadService)
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
            var originFileName = await _fileUploadService.UploadFileAsync(reader, Asp.GetTempPath());
            var d = originFileName.First();
            await _fileUploadService.InsertToDbFromUploadedFileAsync(d, true);

            return Ok();
        }
    }
}
