namespace WebApplication.Interfaces
{
    public interface ICacheFile
    {
        string Pach { get; }
        int MaxCount { get; }
        int Timer { get; }
        void SetParam(string path, int maxCount = 10, int timer = 1000);
    }
}
