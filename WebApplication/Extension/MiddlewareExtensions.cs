using System;
using Microsoft.AspNetCore.Builder;
using WebApplication.Interfaces;
using WebApplication.Middleware;

namespace WebApplication.Extension
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCacheFile(this IApplicationBuilder app, Action<ICacheFileProperties> param)
        {
            var ob = new CacheFileProperties();
            param?.Invoke(ob);
            return app.UseMiddleware<CacheFileMiddleware>(ob);
        }
    }
}
