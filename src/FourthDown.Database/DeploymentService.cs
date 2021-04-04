using System;
using System.Threading;
using System.Threading.Tasks;
using DbUp;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FourthDown.Database
{
    public class DeploymentService : IHostedService
    {
        private readonly ILogger<DeploymentService> _logger;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly DatabaseOptions _databaseOptions;

        private Task _task;

        public DeploymentService(
            ILogger<DeploymentService> logger,
            IOptions<DatabaseOptions> databaseOptions,
            IHostApplicationLifetime appLifetime)
        {
            _logger = logger;
            _databaseOptions = databaseOptions.Value;
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(appLifetime.ApplicationStopping);

            Environment.ExitCode = 0;

            _cancellationTokenSource.Token.Register(() =>
            {
                _logger.LogInformation($"Shutting down {nameof(DeploymentService)}..");
                appLifetime.StopApplication();
            });
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Starting {nameof(DeploymentService)}..");

            if (_task != null)
            {
                throw new InvalidOperationException();
            }

            if (!_cancellationTokenSource.IsCancellationRequested)
            {
                _task = Task.Run(RunDeployment, cancellationToken);
            }

            _logger.LogInformation($"Started {nameof(DeploymentService)}..");

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Stopping {nameof(DeploymentService)}..");

            _cancellationTokenSource.Cancel();

            var runningTask = Interlocked.Exchange(ref _task, null);
            if (runningTask != null)
            {
                await runningTask;
            }

            _logger.LogInformation($"Stopped {nameof(DeploymentService)}..");
        }

        private void RunDeployment()
        {
            EnsureDatabase.For.SqlDatabase(_databaseOptions.ConnectionString);

            var upgradeEngine = DeployChanges.To
                .SqlDatabase(_databaseOptions.ConnectionString)
                .WithScriptsFromFileSystem(_databaseOptions.SchemaLocation)
                .WithTransaction()
                .LogScriptOutput()
                .LogToConsole();

            var upgradeBuilder = upgradeEngine.Build();

            _logger.LogInformation("Scripts to execute: ");
            foreach (var sqlScript in upgradeBuilder.GetScriptsToExecute())
            {
                _logger.LogInformation(sqlScript.Name);
            }

            if (!upgradeBuilder.IsUpgradeRequired())
            {
                _logger.LogError("Database upgrade is not required");
                _cancellationTokenSource.Cancel();
                Environment.ExitCode = 1;
            }

            var upgradeResult = upgradeBuilder.PerformUpgrade();

            if (!upgradeResult.Successful)
            {
                _logger.LogCritical(upgradeResult.Error.Message);
                Environment.ExitCode = 1;
            }
            else
            {
                _logger.LogInformation("Database successfully upgraded!");
                Environment.ExitCode = 0;
            }
            
            _cancellationTokenSource.Cancel();
        }
    }
}