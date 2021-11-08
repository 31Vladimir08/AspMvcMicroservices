using System;

namespace WebApplication.Interfaces
{
    public interface ICacheFileProperties
    {
        string Pach { get; }
        int MaxCount { get; }
        TimeSpan CacheExpirationTime { get; }
        void SetParam(string path, int maxCount = 10, TimeSpan? cacheExpirationTime = null);
    }
}
