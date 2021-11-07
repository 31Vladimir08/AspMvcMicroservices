using System;
using System.IO;
using System.Linq;
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
        private readonly SemaphoreSlim _semaphoreSlim;
        public static readonly object HttpContextItemsCacheFileMiddlewareKey = new();
        private FileSerialazation FileSerialazation { get; set; }

        public CacheFileMiddleware(RequestDelegate next, IWebHostEnvironment env, ICacheFileProperties ob)
        {
            _next = next;
            _env = env;
            _ob = ob;
            _xmlSerializer = new XmlSerializer(typeof(FileSerialazation));
            _semaphoreSlim = new SemaphoreSlim(1);
            FileSerialazation = GetImagesDeserialize();
            DeleteOldFiles();
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
            await Task.Run(() =>
            {
                var route = context.Request.RouteValues;
                if (string.IsNullOrWhiteSpace(_ob.Pach) || route["controller"]?.ToString() != "Category" ||
                    route["action"]?.ToString() != "GetImage")
                    return;
                var categoryId = SetCategoryIdForFile(context.Request.Path);
                if (FileSerialazation.Pictures.Count >= _ob.MaxCount && FileSerialazation.Pictures.All(x => x.CategoryID != categoryId))
                    return;

                if (FileSerialazation.Pictures.All(x => x.CategoryID != categoryId))
                {
                    FileSerialazation.Pictures.Add(new DataSerialazation()
                    {
                        CategoryID = categoryId,
                        DateOfLastReading = DateTime.Now
                    });
                }
                else
                {
                    var t = FileSerialazation.Pictures.FirstOrDefault(x => x.CategoryID != categoryId);
                    if (t != null)
                        t.DateOfLastReading = DateTime.Now;
                }

                var fileInf = new FileInfo($"{_ob.Pach}\\{categoryId}.png");
                if (fileInf.Exists)
                {
                    ImagesSerialize();
                    return;
                }
                   

                using (var fileStream = new FileStream($"{_ob.Pach}\\{categoryId}.png",
                FileMode.Create))
                {
                    fileStream.Lock(0, fileStream.Length);
                    memory.WriteTo(fileStream);
                    byte[] array = new byte[fileStream.Length];
                    fileStream.Read(array, 0, array.Length);
                }

                ImagesSerialize();
                memory.Position = 0;
            });
        }

        private async Task GetFileFromCacheAsync(HttpContext context)
        {
            await Task.Run(
                () =>
                {
                    var route = context.Request.RouteValues;
                    if (string.IsNullOrWhiteSpace(_ob.Pach) || route["controller"]?.ToString() != "Category" ||
                        route["action"]?.ToString() != "GetImage") 
                        return;
                    var categoryId = SetCategoryIdForFile(context.Request.Path);
                    try
                    {
                        using (var fileStream = new FileStream($"{_ob.Pach}/{categoryId}.png",
                            FileMode.Open, FileAccess.Read))
                        {
                            fileStream.Lock(0, fileStream.Length);
                            FileSerialazation = GetImagesDeserialize();
                            var image = FileSerialazation.Pictures.FirstOrDefault(x => x.CategoryID == categoryId);
                            image.DateOfLastReading = DateTime.Now;
                            byte[] array = new byte[fileStream.Length];
                            fileStream.Read(array, 0, array.Length);
                            context.Items[HttpContextItemsCacheFileMiddlewareKey] = array;
                        }
                    }
                    catch (FileNotFoundException e)
                    {
                        //ignore
                    }
                });
        }

        private FileSerialazation GetImagesDeserialize()
        {
            var fileInf = new FileInfo($"{_ob.Pach}\\{SERIALIZATION_FILE_NAME}").Directory;
            if (fileInf is {Exists: false})
            {
                return new FileSerialazation();
            }
            using (var fileStream = new FileStream($"{_ob.Pach}\\{SERIALIZATION_FILE_NAME}",
                FileMode.OpenOrCreate, FileAccess.Read))
            {
                fileStream.Lock(0, fileStream.Length);
                var res = fileStream.Length == 0 ? new FileSerialazation() : _xmlSerializer.Deserialize(fileStream) as FileSerialazation;
                return res;
            }
        }
        
        private void ImagesSerialize()
        {
            _semaphoreSlim.Wait();
            using (var fileStream = new FileStream($"{_ob.Pach}\\{SERIALIZATION_FILE_NAME}",
                FileMode.Truncate))
            {
                fileStream.Lock(0, fileStream.Length);
                _xmlSerializer.Serialize(fileStream, FileSerialazation);
            }
            _semaphoreSlim.Release();
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

        private void DeleteOldFiles(CancellationToken token = default)
        { 
            Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(10000);
                    token.ThrowIfCancellationRequested();
                    var images = GetImagesDeserialize();
                    var isImagesSerialize = false;
                    var _ = images.Pictures
                        .Where(
                        x =>
                        {
                            if (DateTime.Now.Subtract(x.DateOfLastReading) <= _ob.CacheExpirationTime) 
                                return false;
                            var fileInf = new FileInfo($"{_ob.Pach}\\{x.CategoryID}.png");
                            if (fileInf.Exists)
                                fileInf.Delete();
                            if (FileSerialazation.Pictures.Contains(x))
                            {
                                FileSerialazation.Pictures.Remove(x);
                            }
                            isImagesSerialize = true;
                            return true;
                        }).ToList();

                    if (!isImagesSerialize)
                        continue;
                    
                    ImagesSerialize();
                }
            }, token);
        }
    }
}
