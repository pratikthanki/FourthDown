using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FourthDown.Collector
{
    public class CollectorService : IHostedService
    {
        private readonly ILogger<CollectorService> _logger;
        private readonly CancellationTokenSource _cancellationTokenSource;

        private Task _task;

        public CollectorService(
            ILogger<CollectorService> logger,
            IHostApplicationLifetime appLifetime)
        {
            _logger = logger;
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(appLifetime.ApplicationStopping);

            Environment.ExitCode = 0;

            _cancellationTokenSource.Token.Register(() =>
            {
                _logger.LogInformation($"Shutting down {nameof(CollectorService)}..");
                appLifetime.StopApplication();
            });
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Starting {nameof(CollectorService)}..");

            if (_task != null)
                throw new InvalidOperationException();

            if (!_cancellationTokenSource.IsCancellationRequested)
                _task = Task.Run(RunAsync, cancellationToken);

            _logger.LogInformation($"Starting {nameof(CollectorService)}..");

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Stopping {nameof(CollectorService)}..");

            _cancellationTokenSource.Cancel();
            var runningTask = Interlocked.Exchange(ref _task, null);
            if (runningTask != null)
                await runningTask;

            _logger.LogInformation($"Stopped {nameof(CollectorService)}..");
        }

        private async Task RunAsync()
        {
            try
            {
                // do something
            }
            catch (Exception exception)
            {
                LogException(exception.ToString());
            }

            _cancellationTokenSource.Cancel();

            Environment.ExitCode = 0;
        }

        private void LogException(string exception)
        {
            _logger.LogCritical(exception);
            _cancellationTokenSource.Cancel();

            Environment.ExitCode = 1;
        }
    }
}