using Microsoft.AspNetCore.Mvc.Filters;

namespace Fias.Api.Filters
{
    public class UploadCallsActionFilter : IAsyncActionFilter
    {
        private readonly ILogger<UploadCallsActionFilter> _loger;
        private readonly string _tempDirectory;

        public UploadCallsActionFilter(ILogger<UploadCallsActionFilter> loger)
        {
            _loger = loger;
            _tempDirectory = Asp.GetAspDirectoryQueryTempPath();
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            context.HttpContext.Request.Headers.Add("temp_directory", _tempDirectory);
            Directory.CreateDirectory(_tempDirectory);
            _loger.LogInformation($"{_tempDirectory}: CREATE DIRECTORY");

            await next();

            if (Directory.Exists(_tempDirectory))
            {
                Directory.Delete(_tempDirectory, true);
                _loger.LogInformation($"{_tempDirectory}: DELETE DIRECTORY");
            }
        }
    }
}
