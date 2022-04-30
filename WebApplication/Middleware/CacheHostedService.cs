using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using WebApplication.Interfaces;

namespace WebApplication.Middleware
{
    public class CacheHostedService : IHostedService
    {
        private readonly IFileCacheWork _fileCacheWork;

        public CacheHostedService (IWebHostEnvironment env)
        {
            //TODO Temporary decision
            var ob = new CacheFileProperties();
            ob.SetParam(
                    Path.Combine(env.ContentRootPath, "Cash"),
                    cacheExpirationTime: TimeSpan.FromSeconds(5));
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
                    throw new System.NotImplementedException();
                }

                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
