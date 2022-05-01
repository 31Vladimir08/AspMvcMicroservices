using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace WebApplication.Interfaces
{
    public interface IFileCacheWork
    {
        Task DeleteOldFilesAsync(CancellationToken token = default);
        Task DeleteFileAsync(string patch);
        Task AddFileToCacheAsync(MemoryStream memory, string patch);
        Task<byte[]> GetFileFromCacheAsync(string patch);
    }
}
