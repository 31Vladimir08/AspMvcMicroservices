﻿using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using WebApplication.Interfaces;

namespace WebApplication.Middleware
{
    public class CacheHostedService : IHostedService
    {
        private readonly IFileCacheWork _fileCacheWork;

        public CacheHostedService (IWebHostEnvironment env, IDistributedCache cache, IConfiguration configuration)
        {
            var ob = new CacheFileProperties();
            ob.SetParam(
                    Path.Combine(env.ContentRootPath, "Cash"),
                    maxCount: int.Parse(configuration["CacheFileProperties:MaxCountFiles"]),
                    cacheExpirationTime: TimeSpan.FromMinutes(double.Parse(configuration["CacheFileProperties:CacheExpirationTimeMinutes"])));
            _fileCacheWork = new FileCacheWork(ob, cache);
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
                catch (IOException ex)
                {
                    // ignore
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
