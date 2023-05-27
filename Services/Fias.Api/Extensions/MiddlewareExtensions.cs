using Fias.Api.Middlewares;

namespace Fias.Api.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseException(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
