﻿using System;
using System.IO;

using WebApplication.Interfaces;

namespace WebApplication.Middleware
{
    public class CacheFileProperties : ICacheFileProperties
    {
        public string Path  { get; private set; }
        public int MaxCount { get; private set; }
        public TimeSpan CacheExpirationTime { get; private set; }

        public void SetParam(string path, int maxCount = 10, TimeSpan? cacheExpirationTime = null)
        {
            var directory = new DirectoryInfo(path);
            if (!directory.Exists)
            {
                Directory.CreateDirectory(path);
            }

            Path = path;
            MaxCount = maxCount;
            CacheExpirationTime = cacheExpirationTime ?? TimeSpan.FromMinutes(10);
        }
    }
}
