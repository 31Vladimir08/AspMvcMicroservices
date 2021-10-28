using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using WebApplication.Interfaces;

namespace WebApplication.Middleware
{
    public class CacheFileMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ICacheFile _ob;
        private readonly IWebHostEnvironment _env;

        public CacheFileMiddleware(RequestDelegate next, IWebHostEnvironment env, ICacheFile ob)
        {
            _next = next;
            _env = env;
            _ob = ob;
        }

        public async Task Invoke(HttpContext context)
        {
            await GetFileFromCacheAsync(context);
            await AddFileToCacheAsync(context);
            await _next(context);
        }

        private async Task AddFileToCacheAsync(HttpContext context)
        {
            await Task.Run(() =>
            {
                if (string.IsNullOrWhiteSpace(_ob.Pach) || !_ob.Pach.Contains("/category/getpicture") ||
                    context.Request.Method != "POST") 
                    return;

                var file = context.Request.Form.Files.FirstOrDefault();
                if (file?.ContentType != "image/png")
                    return;

                var name = SetNameForFile(_ob.Pach);

                using (var fileStream = new FileStream($"{_env.ContentRootPath}/wwwroot/images/{name}.png",
                    FileMode.OpenOrCreate))
                {
                    file.CopyTo(fileStream);
                    var array = new byte[fileStream.Length];
                    fileStream.Write(array, 0, array.Length);
                }
            });
        }

        private async Task GetFileFromCacheAsync(HttpContext context)
        {
            await Task.Run(
                () =>
                {
                    if (string.IsNullOrWhiteSpace(_ob.Pach) || !_ob.Pach.Contains("/category/getpicture") ||
                        context.Request.Method != "GET")
                        return;

                    var name = SetNameForFile(_ob.Pach);
                    try
                    {
                        using (var fileStream = new FileStream($"{_env.ContentRootPath}/wwwroot/images/{name}.png",
                            FileMode.Open, FileAccess.Read))
                        {
                            byte[] array = new byte[fileStream.Length];
                        }
                    }
                    catch (FileNotFoundException e)
                    {
                        //Console.WriteLine(e);
                        //throw;
                    }
                    
                });
        }

        private string SetNameForFile(string path)
        {
            char[] arr = path.ToCharArray();
            Array.Reverse(arr);
            string name = new string(arr);
            name = name.Substring(0, name.IndexOf('/'));
            arr = name.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }
    }
}
