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
    public class FileCacheWork
    {
        private const string SERIALIZATION_FILE_NAME = "Serialization.xml";
        private readonly ICacheFileProperties _ob;
        private readonly XmlSerializer _xmlSerializer;
        private readonly SemaphoreSlim _semaphoreSlim;

        private FileSerialazation FileSerialazation { get; set; }
        public FileCacheWork(ICacheFileProperties ob)
        {
            _ob = ob;
            _xmlSerializer = new XmlSerializer(typeof(FileSerialazation));
            _semaphoreSlim = new SemaphoreSlim(1);
        }

        private FileSerialazation GetImagesDeserialize()
        {
            var fileInf = new FileInfo($"{_ob.Pach}/{SERIALIZATION_FILE_NAME}").Directory;
            if (fileInf is { Exists: false })
            {
                return new FileSerialazation();
            }
            using (var fileStream = new FileStream($"{_ob.Pach}/{SERIALIZATION_FILE_NAME}",
                FileMode.OpenOrCreate, FileAccess.Read))
            {
                fileStream.Lock(0, fileStream.Length);
                var res = fileStream.Length == 0 ? new FileSerialazation() : _xmlSerializer.Deserialize(fileStream) as FileSerialazation;
                return res;
            }
        }

        private async Task AddFileToCacheAsync(MemoryStream memory, string patch, bool isGetFile)
        {
            var categoryId = SetCategoryIdForFile(patch);

            if (isGetFile)
            {
                var file = FileSerialazation.Pictures.FirstOrDefault(
                    x =>
                    {
                        if (x.CategoryID != categoryId)
                            return false;
                        var fileInf = new FileInfo($"{_ob.Pach}/{x.CategoryID}.png");
                        if (fileInf.Exists)
                            fileInf.Delete();
                        return true;

                    });
                FileSerialazation.Pictures.Remove(file);
                ImagesSerialize();
                return;
            }

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

            var fileInf = new FileInfo($"{_ob.Pach}/{categoryId}.png");
            if (fileInf.Exists)
            {
                ImagesSerialize();
                return;
            }


            using (var fileStream = new FileStream($"{_ob.Pach}/{categoryId}.png",
                FileMode.Create))
            {
                fileStream.Lock(0, fileStream.Length);
                memory.WriteTo(fileStream);
                byte[] array = new byte[fileStream.Length];
                await fileStream.ReadAsync(array, 0, array.Length);
            }

            ImagesSerialize();
            memory.Position = 0;
        }

        private async Task<byte[]> GetFileFromCacheAsync(string patch)
        {
            var categoryId = SetCategoryIdForFile(patch);
            using (var fileStream = new FileStream($"{_ob.Pach}/{categoryId}.png",
                    FileMode.Open, FileAccess.Read))
            {
                fileStream.Lock(0, fileStream.Length);
                var fSerialazation = GetImagesDeserialize();
                var image = fSerialazation.Pictures.FirstOrDefault(x => x.CategoryID == categoryId);
                image.DateOfLastReading = DateTime.Now;
                byte[] array = new byte[fileStream.Length];
                await fileStream.ReadAsync(array, 0, array.Length);
                FileSerialazation = fSerialazation;
                return array;
            }
        }

        private void ImagesSerialize()
        {
            _semaphoreSlim.Wait();
            using (var fileStream = new FileStream($"{_ob.Pach}/{SERIALIZATION_FILE_NAME}",
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
    }
}
