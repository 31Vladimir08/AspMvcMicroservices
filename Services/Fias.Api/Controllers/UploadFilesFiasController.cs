using Fias.Api.Exceptions;
using Fias.Api.Filters;
using Fias.Api.Interfaces.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;

namespace Fias.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ServiceFilter(typeof(UploadCallsActionFilter))]
    public class UploadFilesFiasController : ControllerBase
    {
        private const string KEY_DIRECTORY_NAME = "temp_directory";
        private readonly IFileService _fileUploadService;

        public UploadFilesFiasController(
            IFileService fileUploadService)
        {
            _fileUploadService = fileUploadService ?? throw new ArgumentNullException(nameof(fileUploadService));
        }

        [HttpPost]
        [Route("updateDataBaseFromFile")]
        public async Task<IActionResult> UpdateDbFromFile()
        {
            var tempDirectory = HttpContext.Request.Headers[KEY_DIRECTORY_NAME].ToString();
            if (string.IsNullOrWhiteSpace(tempDirectory))
                throw new UserException("Empty бля");
            if (string.IsNullOrWhiteSpace(Request.ContentType))
                throw new UserException("DGDGDGDG");
            var boundary = HeaderUtilities.RemoveQuotes(
                MediaTypeHeaderValue.Parse(Request.ContentType).Boundary
                ).Value ?? throw new UserException("JDJSLSLS");

            var reader = new MultipartReader(boundary, HttpContext.Request.Body);

            var originFileNames = await _fileUploadService.UploadFileAsync(reader, tempDirectory);
            foreach (var file in originFileNames)
            {
                await _fileUploadService.InsertToDbFromUploadedFileAsync(file);
            }

            return Ok();
        }

        [HttpPost]
        [Route("restoreDataBaseFromFile")]
        public async Task<IActionResult> RestoreDbFromFile()
        {
            var tempDirectory = HttpContext.Request.Headers[KEY_DIRECTORY_NAME].ToString();
            if (string.IsNullOrWhiteSpace(tempDirectory))
                throw new UserException("Empty бля");
            if (string.IsNullOrWhiteSpace(Request.ContentType))
                throw new UserException("DGDGDGDG");
            var boundary = HeaderUtilities.RemoveQuotes(
                MediaTypeHeaderValue.Parse(Request.ContentType).Boundary
                ).Value ?? throw new UserException("JDJSLSLS");

            var reader = new MultipartReader(boundary, HttpContext.Request.Body);
            var originFileNames = await _fileUploadService.UploadFileAsync(reader, tempDirectory);
            foreach (var file in originFileNames)
            {
                await _fileUploadService.InsertToDbFromUploadedFileAsync(file, true);
            }

            return Ok();
        }
    }
}
