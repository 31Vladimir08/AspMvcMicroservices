using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;

using Newtonsoft.Json;

using WebApplication.Interfaces;

namespace WebApplication.Middleware
{
    public class FileCacheWork : IFileCacheWork
    {
        private readonly ICacheFileProperties _ob;
        private readonly IDistributedCache _cache;

        public FileCacheWork(ICacheFileProperties ob, IDistributedCache cache)
        {
            _ob = ob;
            _cache = cache;
        }

        public async Task DeleteFileAsync(string patch)
        {
            var categoryId = GetCategoryIdFromPath(patch);

            await Task.Run(() =>
            {
                var fileInf = new FileInfo($"{_ob.Path}\\{categoryId}.png");
                if (fileInf.Exists)
                    fileInf.Delete();
            });
        }

        public async Task AddFileToCacheAsync(MemoryStream memory, string patch)
        {
            var countFiles = GetCountFilesInFolder(_ob.Path);
            if (countFiles >= _ob.MaxCount)
                return;
            var categoryId = GetCategoryIdFromPath(patch);
            var dataString = await _cache.GetStringAsync(categoryId);
            if (string.IsNullOrWhiteSpace(dataString))
            {
                var json = JsonConvert.SerializeObject(new FileInCasheDataSerialazation()
                {
                    CategoryID = categoryId,
                    DateOfLastReading = DateTime.Now
                });

                await _cache.SetStringAsync(categoryId, json);
            }
            else
            {
                var obj = JsonConvert.DeserializeObject<FileInCasheDataSerialazation>(dataString);
                obj.DateOfLastReading = DateTime.Now;
                var json = JsonConvert.SerializeObject(obj);
                await _cache.SetStringAsync(categoryId, json);
            }

            using (var fileStream = new FileStream($"{_ob.Path}/{categoryId}.png",
                FileMode.CreateNew))
            {
                fileStream.Lock(0, fileStream.Length);
                memory.WriteTo(fileStream);
                byte[] array = new byte[fileStream.Length];
                await fileStream.ReadAsync(array, 0, array.Length);
            }
            memory.Position = 0;
        }

        public async Task<byte[]> GetFileFromCacheAsync(string patch)
        {
            var categoryId = GetCategoryIdFromPath(patch);
            using (var fileStream = new FileStream($"{_ob.Path}/{categoryId}.png",
                    FileMode.Open, FileAccess.Read))
            {
                fileStream.Lock(0, fileStream.Length);
                byte[] array = new byte[fileStream.Length];
                await fileStream.ReadAsync(array, 0, array.Length);
                return array;
            }
        }

        public async Task DeleteOldFilesAsync(CancellationToken token = default)
        {
            await Task.Run(async () =>
            {
                token.ThrowIfCancellationRequested();
                var directory = new DirectoryInfo(_ob.Path);
                if (!directory.Exists)
                    return;
                var files = directory.GetFiles();
                Parallel.ForEach(files, x =>
                {
                    var categoryId = GetCategoryIdFromFaleName(x.Name);
                    var json = _cache.GetString(categoryId);
                    if (json == null || string.IsNullOrWhiteSpace(json))
                        x.Delete();
                    else
                    {
                        var obj = JsonConvert.DeserializeObject<FileInCasheDataSerialazation>(json);
                        if (DateTime.Now.Subtract(obj.DateOfLastReading) > _ob.CacheExpirationTime)
                        {
                            x.Delete();
                            _cache.Remove(categoryId);
                        }
                    }
                });
            }, token);
        }

        private string GetCategoryIdFromPath(string path)
        {
            char[] arr = path.ToCharArray();
            Array.Reverse(arr);
            string name = new string(arr);
            name = name.Substring(0, name.IndexOf('/'));
            arr = name.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }

        private string GetCategoryIdFromFaleName(string fileName)
        {
            var r = fileName.Split('.');
            return r[0];
        }

        private int GetCountFilesInFolder(string pathToCashFolder)
        {
            var count = Directory.GetFiles(pathToCashFolder).Length;
            return count;
        }
    }
}
