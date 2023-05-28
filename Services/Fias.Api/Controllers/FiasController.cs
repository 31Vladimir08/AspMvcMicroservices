using Fias.Api.Exceptions;
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
                throw new UserException("DGDGDGDG");
            var boundary = HeaderUtilities.RemoveQuotes(
                MediaTypeHeaderValue.Parse(Request.ContentType).Boundary
                ).Value ?? throw new UserException("JDJSLSLS");

            var reader = new MultipartReader(boundary, HttpContext.Request.Body);

            var originFileNames = await _fileUploadService.UploadFileAsync(reader, Asp.GetTempPath());
            originFileNames.ForEach(async file =>
            {
                await _fileUploadService.InsertToDbFromUploadedFileAsync(file);
            });

            return Ok();
        }

        [HttpPost]
        [Route("restoreDataBaseFromFile")]
        public async Task<IActionResult> RestoreDbFromFile()
        {
            if (string.IsNullOrWhiteSpace(Request.ContentType))
                throw new UserException("DGDGDGDG");
            var boundary = HeaderUtilities.RemoveQuotes(
                MediaTypeHeaderValue.Parse(Request.ContentType).Boundary
                ).Value ?? throw new UserException("JDJSLSLS");

            var reader = new MultipartReader(boundary, HttpContext.Request.Body);
            var originFileName = await _fileUploadService.UploadFileAsync(reader, Asp.GetTempPath());
            originFileName.ForEach(async file =>
            {
                await _fileUploadService.InsertToDbFromUploadedFileAsync(file, true);
            });

            return Ok();
        }
    }
}
