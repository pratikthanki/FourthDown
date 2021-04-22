using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DbUp;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
#pragma warning disable 1998

namespace FourthDown.Database
{
    public class DatabaseOptions
    {
        public string ConnectionString { get; set; }
    }

    public class DeploymentService : BackgroundService
    {
        private readonly ILogger<DeploymentService> _logger;
        private readonly DatabaseOptions _databaseOptions;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public DeploymentService(
            ILogger<DeploymentService> logger,
            IOptions<DatabaseOptions> databaseOptions,
            IHostApplicationLifetime applicationLifetime)
        {
            _logger = logger;
            _databaseOptions = databaseOptions.Value;
            _cancellationTokenSource =
                CancellationTokenSource.CreateLinkedTokenSource(applicationLifetime.ApplicationStarted);

            Environment.ExitCode = 1;

            _cancellationTokenSource.Token.Register(() =>
            {
                _logger.LogCritical("Shutting down database migration...");
                applicationLifetime.StopApplication();
            });
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() => _logger.LogDebug($"Deployment background task is stopping."));

            try
            {
                UpgradeDatabase();
                Environment.ExitCode = 0;
                _cancellationTokenSource.Cancel();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred migrating database");
                _cancellationTokenSource.Cancel();
            }
        }

        private void UpgradeDatabase()
        {
            EnsureDatabase.For.SqlDatabase(_databaseOptions.ConnectionString);
            var upgradeEngine = DeployChanges.To
                .SqlDatabase(_databaseOptions.ConnectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .WithTransaction()
                .LogScriptOutput()
                .Build();

            if (!upgradeEngine.IsUpgradeRequired())
            {
                _logger.LogError("Database upgrade is not required");
            }
            else
            {
                var upgradeResult = upgradeEngine.PerformUpgrade();
                if (!upgradeResult.Successful)
                {
                    _logger.LogCritical(upgradeResult.Error.Message);
                }
            }
        }
    }
}