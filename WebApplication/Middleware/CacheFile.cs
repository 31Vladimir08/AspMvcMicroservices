using WebApplication.Interfaces;

namespace WebApplication.Middleware
{
    public class CacheFile : ICacheFile
    {
        public string Pach  { get; private set; }
        public int MaxCount { get; private set; }
        public int Timer { get; private set; }

        public void SetParam(string path, int maxCount = 10, int timer = 1000)
        {
            Pach = path;
            MaxCount = maxCount;
            Timer = timer;
        }
    }
}
