using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FourthDown.Database
{
    public class DeploymentService : IHostedService
    {
        private readonly ILogger<DeploymentService> _logger;
        private readonly IDatabaseClient _databaseClient;
        private readonly CancellationTokenSource _cancellationTokenSource;

        private Task _task;

        public DeploymentService(
            ILogger<DeploymentService> logger,
            IDatabaseClient databaseClient,
            IHostApplicationLifetime appLifetime)
        {
            _logger = logger;
            _databaseClient = databaseClient;
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
                _task = Task.Run(RunDeployment, cancellationToken);

            _logger.LogInformation($"Starting {nameof(DeploymentService)}..");

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

        private async Task RunDeployment()
        {
            var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (assemblyPath == null)
                throw new Exception($"{nameof(assemblyPath)} not set");

            var path = Path.Combine(assemblyPath, @"../../../Scripts");
            var deploymentScripts = Directory.GetFiles(path);

            var preDeploymentCheck = await _databaseClient.PreDeploymentCheckSuccessful(_cancellationTokenSource.Token);

            if (!preDeploymentCheck)
                LogException("PreDeploymentCheck failed");

            foreach (var script in deploymentScripts)
            {
                _logger.LogInformation($"Migration script found: {script}");
            }

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