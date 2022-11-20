using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;

using WebApplication.Interfaces;

namespace WebApplication.Middleware
{
    public class CacheFileMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ICacheFileProperties _ob;
        public static readonly object HttpContextItemsCacheFileMiddlewareKey = new();
        private readonly IFileCacheWork _fileCacheWork;

        public CacheFileMiddleware(RequestDelegate next, ICacheFileProperties ob, IDistributedCache cache)
        {
            _next = next;
            _ob = ob;
            _fileCacheWork = new FileCacheWork(ob, cache);
        }

        public async Task Invoke(HttpContext context)
        {
            var originalBody = context.Response.Body;

            try
            {
                using (var memStream = new MemoryStream())
                {
                    context.Response.Body = memStream;
                    await GetFileFromCacheAsync(context);

                    await _next(context);

                    memStream.Position = 0;
                    
                    await AddFileToCacheAsync(context, memStream);
                    memStream.Position = 0;
                    await memStream.CopyToAsync(originalBody);
                }

            }
            finally
            {
                context.Response.Body = originalBody;
            }
        }

        private async Task AddFileToCacheAsync(HttpContext context, MemoryStream memory)
        {
            var route = context.Request.RouteValues;
            if (string.IsNullOrWhiteSpace(_ob.Path) || route["controller"]?.ToString() != "Category" ||
                (route["action"]?.ToString() != "GetImage" && route["action"]?.ToString() != "GetPicture"))
                return;

            if (route["action"]?.ToString() == "GetPicture")
            {
                await _fileCacheWork.DeleteFileAsync(context.Request.Path);
                return;
            }

            await _fileCacheWork.AddFileToCacheAsync(memory, context.Request.Path);
        }

        private async Task GetFileFromCacheAsync(HttpContext context)
        {
            var route = context.Request.RouteValues;
            if (string.IsNullOrWhiteSpace(_ob.Path) || route["controller"]?.ToString() != "Category" ||
                route["action"]?.ToString() != "GetImage")
                return;
            try
            {
                var array = await _fileCacheWork.GetFileFromCacheAsync(context.Request.Path);
                context.Items[HttpContextItemsCacheFileMiddlewareKey] = array;
            }
            catch (FileNotFoundException e)
            {
                //ignore
            }
        }
    }
}
