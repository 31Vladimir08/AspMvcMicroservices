using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using WebApplication.Interfaces;
using WebApplication.Middleware;

namespace WebApplication.Extension
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCacheFile(this IApplicationBuilder app, Action<ICacheFile> param)
        {
            var ob = new CacheFile();
            param(ob);
            return app.UseMiddleware<CacheFileMiddleware>(ob);
        }
    }
}
