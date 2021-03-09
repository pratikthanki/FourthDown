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
                throw new InvalidOperationException();

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
                await runningTask;

            _logger.LogInformation($"Stopped {nameof(DeploymentService)}..");
        }

        private void RunDeployment()
        {
            var upgradeBuilder = DeployChanges.To
                .SqlDatabase(_databaseOptions.ConnectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .LogToConsole()
                .Build();

            var scriptsToExecute = upgradeBuilder.GetScriptsToExecute();

            _logger.LogInformation("Scripts to execute: ");
            foreach (var sqlScript in scriptsToExecute)
            {
                _logger.LogInformation(sqlScript.Name);
                
            }

            // var result = upgradeBuilder.PerformUpgrade();
            //
            // if (!result.Successful)
            // {
            //     _logger.LogCritical(result.Error.Message);
            //     _cancellationTokenSource.Cancel();
            //     Environment.ExitCode = 1;
            // }

            Console.ForegroundColor = ConsoleColor.Green;
            _logger.LogInformation("Database successfully upgraded!");
            Console.ResetColor();

            _cancellationTokenSource.Cancel();

            Environment.ExitCode = 0;
        }
    }
}