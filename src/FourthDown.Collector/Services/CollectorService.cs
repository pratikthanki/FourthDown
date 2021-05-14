using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FourthDown.Collector.Services
{
    public class CollectorService : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly ICollectorManager _collectorManager;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public CollectorService(
            ILogger<CollectorService> logger,
            ICollectorManager collectorManager,
            IHostApplicationLifetime appLifetime)
        {
            _logger = logger;
            _collectorManager = collectorManager;
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(appLifetime.ApplicationStopping);

            Environment.ExitCode = 1;

            _cancellationTokenSource.Token.Register(() =>
            {
                _logger.LogInformation($"Shutting down {nameof(CollectorService)}..");
                appLifetime.StopApplication();
            });
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var dataWritten = await _collectorManager.RunAsync(_cancellationTokenSource.Token);
                    if (!dataWritten)
                    {
                        await Task.Delay(TimeSpan.FromMinutes(10), cancellationToken);
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.LogCritical(exception.Message);
                _cancellationTokenSource.Cancel();

                Environment.ExitCode = 1;
            }
            finally
            {
                _cancellationTokenSource.Cancel();
            }
        }
    }
}