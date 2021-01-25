using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Collector.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FourthDown.Collector
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
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"../../../Scripts");
            var files = Directory.GetFiles(path);
            
            foreach(var file in files)
            {
                _logger.LogInformation($"Migration script found: {file}");
            }

            try
            {
                // do something
            }
            catch (Exception exception)
            {
                _logger.LogCritical(exception.ToString());
                _cancellationTokenSource.Cancel();

                Environment.ExitCode = 1;
            }

            _cancellationTokenSource.Cancel();

            Environment.ExitCode = 0;
        }
        
        private static string GetAbsolutePath()
        {
            const string relativePath = @"../../../../Scripts";
            var _dataRoot = new FileInfo(typeof(Program).Assembly.Location);

            Debug.Assert(_dataRoot.Directory != null);

            var assemblyFolderPath = _dataRoot.Directory.FullName;

            var fullPath = Path.Combine(assemblyFolderPath, relativePath);

            return fullPath;
        }
    }
}