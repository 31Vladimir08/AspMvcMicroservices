using System;
using WebApplication.Interfaces;

namespace WebApplication.Middleware
{
    public class CacheFileProperties : ICacheFileProperties
    {
        public string Pach  { get; private set; }
        public int MaxCount { get; private set; }
        public TimeSpan CacheExpirationTime { get; private set; }

        public void SetParam(string path, int maxCount = 10, TimeSpan? cacheExpirationTime = null)
        {
            Pach = path;
            MaxCount = maxCount;
            CacheExpirationTime = cacheExpirationTime ?? TimeSpan.FromMinutes(10);
        }
    }
}
