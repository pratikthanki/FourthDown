using System;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Collector.Repositories;
using FourthDown.Collector.Utilities;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FourthDown.Collector.Service
{
    public class DataCollectorService : IHostedService
    {
        private ILogger _logger;
        private IPlayByPlayRepository _playByPlayRepository;
        private IGameRepository _gameRepository;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly CancellationToken _cancellationToken;
        private Task _task;

        public DataCollectorService(
            ILogger<DataCollectorService> logger,
            IHostApplicationLifetime appLifetime,
            IPlayByPlayRepository playByPlayRepository, IGameRepository gameRepository)
        {
            _logger = logger;
            _playByPlayRepository = playByPlayRepository;
            _gameRepository = gameRepository;

            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(appLifetime.ApplicationStopping);
            _cancellationToken = _cancellationTokenSource.Token;

            Environment.ExitCode = 0;

            _cancellationTokenSource.Token.Register(() =>
            {
                _logger.LogInformation($"Shutting down {nameof(DataCollectorService)}..");
                appLifetime.StopApplication();
            });
        }

        public Task StartAsync(CancellationToken token)
        {
            _logger.LogInformation($"Starting {nameof(DataCollectorService)}..");

            if (_task != null)
                throw new InvalidOperationException();

            if (!_cancellationTokenSource.IsCancellationRequested)
                _task = Task.Run(RunService, token);

            _logger.LogInformation($"Starting {nameof(DataCollectorService)}..");

            return Task.CompletedTask;
        }

        private async Task RunService()
        {
            try
            {
                var games = await _gameRepository.GetGames(_cancellationTokenSource.Token);

                const string fileName = "play_by_play_2020";
                var plays = _playByPlayRepository.ReadPlays();
                JsonFileWriter.Write(plays, fileName);
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

        public async Task StopAsync(CancellationToken token)
        {
            _logger.LogInformation($"Stopping {nameof(DataCollectorService)}..");

            _cancellationTokenSource.Cancel();
            var runningTask = Interlocked.Exchange(ref _task, null);
            if (runningTask != null)
                await runningTask;

            _logger.LogInformation($"Stopped {nameof(DataCollectorService)}..");
        }
    }
}