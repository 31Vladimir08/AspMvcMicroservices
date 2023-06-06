using System.Collections.Concurrent;

using Fias.Api.Interfaces.Services;
using Fias.Api.Models.File;
using Fias.Api.ViewModels.Models;

using Microsoft.AspNetCore.WebUtilities;

namespace Fias.Api.HostedServices
{
    public class FiasUpdateDbService : BackgroundService
    {
        private readonly ILogger<FiasUpdateDbService> _loger;
        private readonly IServiceProvider _serviceProvider;
        private static readonly ConcurrentDictionary<string, bool> _sessionsRun;
        private static readonly Semaphore _uploadFileSemaphore;
        private static readonly Semaphore _updateDbFromFileSemaphore;
        private event Action<(string tempDirectory, FileViewModel fileVM, bool isRestoreDb)> _executeAsyncNotify;

        static FiasUpdateDbService()
        {
            _sessionsRun = new ConcurrentDictionary<string, bool>();
            _uploadFileSemaphore = new Semaphore(1, 1);
            _updateDbFromFileSemaphore = new Semaphore(1, 1);
        }

        public FiasUpdateDbService(
            IServiceProvider serviceProvider,
            ILogger<FiasUpdateDbService> loger)
        {
            _serviceProvider = serviceProvider;
            _loger = loger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _executeAsyncNotify += async (x) =>
                {
                    _updateDbFromFileSemaphore.WaitOne();
                    try
                    {
                        using (var scope = _serviceProvider.CreateScope())
                        {
                            foreach (var file in x.fileVM.TempFiles)
                            {
                                var service = scope.ServiceProvider.GetRequiredService<IXmlService>();
                                await service.InsertToDbFromArchiveAsync(file, x.fileVM.SelectedRegions, x.isRestoreDb);
                            }
                        }
                    }
                    finally
                    {
                        if (Directory.Exists(x.tempDirectory))
                        {
                            Directory.Delete(x.tempDirectory, true);
                        }

                        _sessionsRun.TryRemove(new KeyValuePair<string, bool>("run", true));
                    }
                    _updateDbFromFileSemaphore.Release();
                };
            }
            catch (Exception ex)
            {
                _loger.LogError($"{ex.Message} {ex.StackTrace} {ex.InnerException}");
            }

            return Task.CompletedTask;
        }

        public async Task<bool> StartEventUpdateDbFromFileExecuteAsync(MultipartReader reader, string tempDirectory, bool isRestoreDb = false)
        {
            var isTrue = _sessionsRun.TryAdd("run", true);
            if (isTrue)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    _uploadFileSemaphore.WaitOne();
                    var service = scope.ServiceProvider.GetRequiredService<IFileService>();
                    var fileVM = await service.UploadFileAsync(reader, tempDirectory);
                    _uploadFileSemaphore.Release();

                    _executeAsyncNotify?.Invoke((tempDirectory, fileVM, isRestoreDb));
                    
                }
                return true;
            }
            else
                return false;
        }
    }
}
