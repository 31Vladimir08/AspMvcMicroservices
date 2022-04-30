using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Microsoft.AspNetCore.Http;

using WebApplication.Interfaces;

namespace WebApplication.Middleware
{
    public class FileCacheWork : IFileCacheWork
    {
        private const string SERIALIZATION_FILE_NAME = "Serialization.xml";
        private readonly ICacheFileProperties _ob;
        private readonly XmlSerializer _xmlSerializer;
        private readonly SemaphoreSlim _semaphoreSlim;

        public FileCacheWork(ICacheFileProperties ob)
        {
            _ob = ob;
            _xmlSerializer = new XmlSerializer(typeof(FileSerialazation));
            _semaphoreSlim = new SemaphoreSlim(1);
        }

        private async Task<FileSerialazation> GetImagesDeserializeAsync()
        {
            return await Task.Run(() =>
            {
                var fileInf = new FileInfo($"{_ob.Path}/{SERIALIZATION_FILE_NAME}").Directory;
                if (fileInf is { Exists: false })
                {
                    return new FileSerialazation();
                }
                using (var fileStream = new FileStream($"{_ob.Path}/{SERIALIZATION_FILE_NAME}",
                    FileMode.OpenOrCreate, FileAccess.Read))
                {
                    fileStream.Lock(0, fileStream.Length);
                    var res = fileStream.Length == 0 ? new FileSerialazation() : _xmlSerializer.Deserialize(fileStream) as FileSerialazation;
                    return res;
                }
            }); 
        }

        public async Task DeleteFileAsync(string patch)
        {
            var categoryId = SetCategoryIdForFile(patch);

            var f = await GetImagesDeserializeAsync();

            await Task.Run(() =>
            {
                var file = f.Pictures.FirstOrDefault(
                  x =>
                  {
                      if (x.CategoryID != categoryId)
                          return false;
                      var fileInf = new FileInfo($"{_ob.Path}/{x.CategoryID}.png");
                      if (fileInf.Exists)
                          fileInf.Delete();
                      return true;

                  });
                f.Pictures.Remove(file);
                ImagesSerialize(f);
            });
        }

        public async Task AddFileToCacheAsync(MemoryStream memory, string patch)
        {
            var categoryId = SetCategoryIdForFile(patch);

            var f = await GetImagesDeserializeAsync();

            if (f.Pictures.Count >= _ob.MaxCount && f.Pictures.All(x => x.CategoryID != categoryId))
                return;

            if (f.Pictures.All(x => x.CategoryID != categoryId))
            {
                f.Pictures.Add(new DataSerialazation()
                {
                    CategoryID = categoryId,
                    DateOfLastReading = DateTime.Now
                });
            }
            else
            {
                var t = f.Pictures.FirstOrDefault(x => x.CategoryID != categoryId);
                if (t != null)
                    t.DateOfLastReading = DateTime.Now;
            }

            var fileInf = new FileInfo($"{_ob.Path}/{categoryId}.png");
            if (fileInf.Exists)
            {
                ImagesSerialize(f);
                return;
            }


            using (var fileStream = new FileStream($"{_ob.Path}/{categoryId}.png",
                FileMode.Create))
            {
                fileStream.Lock(0, fileStream.Length);
                memory.WriteTo(fileStream);
                byte[] array = new byte[fileStream.Length];
                await fileStream.ReadAsync(array, 0, array.Length);
            }

            ImagesSerialize(f);
            memory.Position = 0;
        }

        public async Task<byte[]> GetFileFromCacheAsync(string patch)
        {
            var categoryId = SetCategoryIdForFile(patch);
            using (var fileStream = new FileStream($"{_ob.Path}/{categoryId}.png",
                    FileMode.Open, FileAccess.Read))
            {
                fileStream.Lock(0, fileStream.Length);
                var fSerialazation = await GetImagesDeserializeAsync();
                var image = fSerialazation.Pictures.FirstOrDefault(x => x.CategoryID == categoryId);
                image.DateOfLastReading = DateTime.Now;
                byte[] array = new byte[fileStream.Length];
                await fileStream.ReadAsync(array, 0, array.Length);
                return array;
            }
        }

        private void ImagesSerialize(FileSerialazation fileSerialazationDs)
        {
            _semaphoreSlim.Wait();
            using (var fileStream = new FileStream($"{_ob.Path}/{SERIALIZATION_FILE_NAME}",
                FileMode.Truncate))
            {
                fileStream.Lock(0, fileStream.Length);
                _xmlSerializer.Serialize(fileStream, fileSerialazationDs);
            }
            _semaphoreSlim.Release();
        }

        public async Task DeleteOldFilesAsync(CancellationToken token = default)
        {
            await Task.Run(async () =>
            {
                token.ThrowIfCancellationRequested();
                var images = await GetImagesDeserializeAsync();
                var isImagesSerialize = false;
                var d =_ob.CacheExpirationTime;
                var t = images.Pictures
                    .Where(
                    x =>
                    {
                        if (DateTime.Now.Subtract(x.DateOfLastReading) <= _ob.CacheExpirationTime)
                            return true;
                        var fileInf = new FileInfo($"{_ob.Path}/{x.CategoryID}.png");
                        if (fileInf.Exists)
                            fileInf.Delete();
                        isImagesSerialize = true;
                        return false;
                    }).ToList();

                if (!isImagesSerialize)
                    return;
                images.Pictures = t;
                ImagesSerialize(images);
            }, token);
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
    }
}
