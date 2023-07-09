using Microsoft.AspNetCore.Mvc.Filters;

namespace Fias.Api.Filters
{
    public class UploadCallsActionFilter : IAsyncActionFilter
    {
        private readonly string _tempDirectory;

        public UploadCallsActionFilter()
        {
            _tempDirectory = Asp.GetAspDirectoryQueryTempPath();
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            context.HttpContext.Request.Headers.Add("temp_directory", _tempDirectory);
            await next();
        }
    }
}
