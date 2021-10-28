namespace WebApplication.Interfaces
{
    public interface ICacheFile
    {
        void SetParam(string path, int maxCount = 10, int timer = 1000);
    }
}
