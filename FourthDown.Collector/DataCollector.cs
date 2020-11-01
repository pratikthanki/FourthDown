using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using FourthDown.Collector.Repositories;
using FourthDown.Collector.Utilities;

namespace FourthDown.Collector
{
    public class DataCollector : IHostedService
    {
        private ILogger _logger;
        private IPlayByPlayRepository _playByPlayRepository;
        private IGameRepository _gameRepository;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly CancellationToken _cancellationToken;
        private Task _task;

        public DataCollector(
            ILogger<DataCollector> logger,
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
                _logger.LogInformation($"Shutting down {nameof(DataCollector)}..");
                appLifetime.StopApplication();
            });
        }

        public Task StartAsync(CancellationToken token)
        {
            _logger.LogInformation($"Starting {nameof(DataCollector)}..");

            if (_task != null)
                throw new InvalidOperationException();

            if (!_cancellationTokenSource.IsCancellationRequested)
                _task = Task.Run(RunService, token);

            _logger.LogInformation($"Starting {nameof(DataCollector)}..");

            return Task.CompletedTask;
        }

        private async Task RunService()
        {
            try
            {
                var games = await _gameRepository.GetGames(2020, _cancellationTokenSource.Token);

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
            _logger.LogInformation($"Stopping {nameof(DataCollector)}..");

            _cancellationTokenSource.Cancel();
            var runningTask = Interlocked.Exchange(ref _task, null);
            if (runningTask != null)
                await runningTask;

            _logger.LogInformation($"Stopped {nameof(DataCollector)}..");
        }
    }
}