using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;

using WebApplication.Interfaces;

namespace WebApplication.Middleware
{
    public class CacheHostedService : IHostedService
    {
        private readonly ICacheFileProperties _ob;
        private readonly IFileCacheWork _fileCacheWork;

        public CacheHostedService (ICacheFileProperties ob)
        {
            _ob = ob;
            _fileCacheWork = new FileCacheWork(ob);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _ = DeleteOldCacheFile(cancellationToken);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        private async Task DeleteOldCacheFile(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _fileCacheWork.DeleteOldFilesAsync();
                }
                catch (Exception ex)
                {
                    // обработка ошибки однократного неуспешного выполнения фоновой задачи
                }

                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
