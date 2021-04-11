using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DbUp;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FourthDown.Database
{
    public class DeploymentService : BackgroundService
    {
        private readonly ILogger<DeploymentService> _logger;
        private readonly DatabaseOptions _databaseOptions;

        public DeploymentService(
            ILogger<DeploymentService> logger,
            IOptions<DatabaseOptions> databaseOptions)
        {
            _logger = logger;
            _databaseOptions = databaseOptions.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() => _logger.LogDebug($" Deployment background task is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                EnsureDatabase.For.SqlDatabase(_databaseOptions.ConnectionString);

                var upgradeBuilder = DeployChanges.To
                    .SqlDatabase(_databaseOptions.ConnectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                    .WithTransaction()
                    .LogScriptOutput()
                    .LogToConsole()
                    .Build();

                if (!upgradeBuilder.IsUpgradeRequired())
                {
                    _logger.LogError("Database upgrade is not required");
                    return;
                }


                var upgradeResult = upgradeBuilder.PerformUpgrade();

                if (!upgradeResult.Successful)
                {
                    _logger.LogCritical(upgradeResult.Error.Message);
                }
                else
                {
                    _logger.LogInformation("Database successfully upgraded!");
                }
            }
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{DateTime.Now}: Deployment started.");

            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{DateTime.Now}:Deployment stopped.");

            return base.StopAsync(cancellationToken);
        }

        public override void Dispose()
        {
            _logger.LogInformation($"{DateTime.Now}:Worker disposed.");

            base.Dispose();
        }
    }
}