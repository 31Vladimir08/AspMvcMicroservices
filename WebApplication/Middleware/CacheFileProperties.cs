using WebApplication.Interfaces;

namespace WebApplication.Middleware
{
    public class CacheFileProperties : ICacheFileProperties
    {
        public string Pach  { get; private set; }
        public int MaxCount { get; private set; }
        public int Minutes { get; private set; }

        public void SetParam(string path, int maxCount = 10, int minutes = 1000)
        {
            Pach = path;
            MaxCount = maxCount;
            Minutes = minutes;
        }
    }
}
