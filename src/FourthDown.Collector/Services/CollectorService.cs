using System;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Collector.Database;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FourthDown.Collector.Services
{
    public class CollectorService : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IMigrator _migrator;
        private readonly ICollectorManager _collectorManager;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly CancellationToken _cancellationToken;
        private readonly TimeSpan _timeDelay = TimeSpan.FromMinutes(10);

        public CollectorService(
            ILogger<CollectorService> logger,
            IMigrator migrator,
            ICollectorManager collectorManager,
            IHostApplicationLifetime appLifetime)
        {
            _logger = logger;
            _migrator = migrator;
            _collectorManager = collectorManager;
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(appLifetime.ApplicationStopping);
            _cancellationToken = _cancellationTokenSource.Token;

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
                // Upgrade database if that is required
                var migrationResult = _migrator.UpgradeDatabase();
                if (migrationResult == MigrationResult.Failure)
                {
                    _logger.LogCritical("Database migration failed");
                    _cancellationTokenSource.Cancel();
                }

                while (!_cancellationToken.IsCancellationRequested)
                {
                    await Task.Run(() => _collectorManager.ProcessGamesAsync(_cancellationToken), _cancellationToken);

                    var isDataWritten = await _collectorManager.TryGetGamesAsync(_cancellationToken);
                    if (!isDataWritten)
                    {
                        await Task.Delay(_timeDelay, _cancellationToken);
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