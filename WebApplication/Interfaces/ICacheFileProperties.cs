namespace WebApplication.Interfaces
{
    public interface ICacheFileProperties
    {
        string Pach { get; }
        int MaxCount { get; }
        int Minutes { get; }
        void SetParam(string path, int maxCount = 10, int minutes = 1000);
    }
}
