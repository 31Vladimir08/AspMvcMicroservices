using Fias.Api.Exceptions;
using Fias.Api.Filters;
using Fias.Api.HostedServices;
using Fias.Api.ViewModels.Filters;

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
        private readonly FiasUpdateDbService _fiasUpdateDbService;

        public UploadFilesFiasController(
            FiasUpdateDbService fiasUpdateDbService)
        {
            _fiasUpdateDbService = fiasUpdateDbService ?? throw new ArgumentNullException(nameof(fiasUpdateDbService));
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
            var isRun = await _fiasUpdateDbService.StartEventUpdateDbFromFileExecuteAsync(reader, tempDirectory);
            return isRun
                ? Ok(new { Status = "ok" })
                : Ok(new { Status = "run" });
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

            var isRun = await _fiasUpdateDbService.StartEventUpdateDbFromFileExecuteAsync(reader, tempDirectory, true);
            return isRun
                ? Ok(new { Status = "ok" })
                : Ok(new { Status = "run" });
        }
    }
}
