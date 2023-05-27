using System.Net;

namespace Fias.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(
            RequestDelegate requestDelegate,
            ILogger<ExceptionMiddleware> logger)
        {
            _requestDelegate = requestDelegate;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var filePath = Asp.GetTempPath();
            try
            {                
                Directory.CreateDirectory(filePath);
                await _requestDelegate(context);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsync(HttpStatusCode.InternalServerError.ToString());
            }
            finally
            {
                Directory.Delete(filePath, true);
            }
        }
    }
}
