using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using WebApplication.Interfaces;

namespace WebApplication.Middleware
{
    public class CacheFileMiddleware
    {
        private const string SERIALIZATION_FILE_NAME = "Serialization.xml";
        private readonly RequestDelegate _next;
        private readonly ICacheFileProperties _ob;
        private readonly IWebHostEnvironment _env;
        private readonly XmlSerializer _xmlSerializer;
        private Images Images { get; set; }

        public CacheFileMiddleware(RequestDelegate next, IWebHostEnvironment env, ICacheFileProperties ob)
        {
            _next = next;
            _env = env;
            _ob = ob;
            _xmlSerializer = new XmlSerializer(typeof(Images));
            Images = GetImagesDeserialize();
            _ = DeleteOldFilesAsync();
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
                if (string.IsNullOrWhiteSpace(_ob.Pach) || !context.Request.Path.ToString().Contains("/Category/GetPicture") ||
                    context.Request.Method != "POST") 
                    return;

                var file = context.Request.Form.Files.FirstOrDefault();
                if (file?.ContentType != "image/png")
                    return;

                var categoryId = SetCategoryIdForFile(context.Request.Path);

                if (Images.Pictures.Count >= _ob.MaxCount || Images.Pictures.All(x => x.CategoryID != categoryId))
                    return;

                if (Images.Pictures.All(x => x.CategoryID != categoryId))
                {
                    Images.Pictures.Add(new Image()
                    {
                        CategoryID = categoryId,
                        DateOfLastReading = DateTime.Now
                    });
                }
                else
                {
                    var t = Images.Pictures.FirstOrDefault(x => x.CategoryID != categoryId);
                    if (t != null) 
                        t.DateOfLastReading = DateTime.Now;
                }

                using (var fileStream = new FileStream($"{_ob.Pach}\\{categoryId}.png",
                    FileMode.OpenOrCreate))
                {
                    fileStream.Lock(0, fileStream.Length);
                    file.CopyTo(fileStream);
                    var array = new byte[fileStream.Length];

                    fileStream.Write(array, 0, array.Length);
                }

                ImagesSerialize();
            });
        }

        private async Task GetFileFromCacheAsync(HttpContext context)
        {
            await Task.Run(
                () =>
                {
                    if (string.IsNullOrWhiteSpace(_ob.Pach) || !context.Request.Path.ToString().Contains("/Category/GetPicture") ||
                        context.Request.Method != "GET")
                        return;

                    var categoryId = SetCategoryIdForFile(context.Request.Path);
                    try
                    {
                        using (var fileStream = new FileStream($"{_env.ContentRootPath}/wwwroot/images/{categoryId}.png",
                            FileMode.Open, FileAccess.Read))
                        {
                            fileStream.Lock(0, fileStream.Length);
                            Images = GetImagesDeserialize();
                            var image = Images.Pictures.FirstOrDefault(x => x.CategoryID == categoryId);
                            image.DateOfLastReading = DateTime.Now;
                            byte[] array = new byte[fileStream.Length];
                        }
                    }
                    catch (FileNotFoundException e)
                    {
                        //ignore
                    }
                    
                });
        }

        private Images GetImagesDeserialize()
        {
            using (var fileStream = new FileStream($"{_ob.Pach}\\{SERIALIZATION_FILE_NAME}",
                FileMode.OpenOrCreate, FileAccess.Read))
            {
                fileStream.Lock(0, fileStream.Length);
                var res = fileStream.Length == 0 ? new Images() : _xmlSerializer.Deserialize(fileStream) as Images;
                return res;
            }
        }

        private void ImagesSerialize()
        {
            using (var fileStream = new FileStream($"{_ob.Pach}\\{SERIALIZATION_FILE_NAME}",
                FileMode.Truncate))
            {
                fileStream.Lock(0, fileStream.Length);
                _xmlSerializer.Serialize(fileStream, Images);
            }
        }

        private string SetCategoryIdForFile(string path)
        {
            char[] arr = path.ToCharArray();
            Array.Reverse(arr);
            string name = new string(arr);
            name = name.Substring(0, name.IndexOf('/'));
            arr = name.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }

        private async Task DeleteOldFilesAsync(CancellationToken token = default)
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(10000);
                    token.ThrowIfCancellationRequested();
                    var images = GetImagesDeserialize();
                    var d = images.Pictures
                        .AsParallel()
                        .Where(
                        x =>
                        {
                            return (DateTime.Now - x.DateOfLastReading).Minutes > _ob.Minutes;
                        })
                        .ToList();

                    if (!d.Any())
                        continue;

                    d.ForEach(
                        x =>
                        {
                            new FileInfo($"{_ob.Pach}\\{x.CategoryID}.png").Delete();
                            Images.Pictures.Remove(x);
                        });
                    
                    ImagesSerialize();
                }
            });
        }
    }
}
