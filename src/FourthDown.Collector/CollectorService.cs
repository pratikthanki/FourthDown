using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Shared.Models;
using FourthDown.Shared.Repositories;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FourthDown.Collector
{
    public class CollectorService : IHostedService
    {
        private readonly ILogger<CollectorService> _logger;
        private readonly IDatabaseClient<GameDetail> _databaseClient;
        private readonly IGameRepository _gameRepository;
        private readonly IGamePlayRepository _gamePlayRepository;

        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly CancellationToken _cancellationToken;

        private Task _task;

        public CollectorService(
            ILogger<CollectorService> logger,
            IHostApplicationLifetime appLifetime,
            IDatabaseClient<GameDetail> databaseClient,
            IGameRepository gameRepository,
            IGamePlayRepository gamePlayRepository)
        {
            _logger = logger;
            _databaseClient = databaseClient;
            _gameRepository = gameRepository;
            _gamePlayRepository = gamePlayRepository;

            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(appLifetime.ApplicationStopping);
            _cancellationToken = _cancellationTokenSource.Token;

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
            /*
             * 1. Read games from github csv
             * 2. Check last games written in database
             * 3. Make request to get just those games
             * 4. Insert to database 
             */

            var gameIdsWritten = await _databaseClient.GetGameIds(_cancellationToken);
            var allGames = await _gameRepository.GetGamesAsync(_cancellationToken);

            var gamesToWrite = allGames.SelectMany(x => x.Value).Where(x => !gameIdsWritten.Contains(x.GameId));

            var gameDetailsTask = GetGamePlays(gamesToWrite, _cancellationToken);
            var gameDetails = await gameDetailsTask;

            // _databaseClient.Write();

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

        private async Task<IEnumerable<GameDetail>> GetGamePlays(
            IEnumerable<Game> games, 
            CancellationToken cancellationToken)
        {
            var requests = games
                .Select(game => _gamePlayRepository.GetGamePlaysAsync(game, cancellationToken))
                .ToList();

            //Wait for all the requests to finish
            await Task.WhenAll(requests);

            var gameDetails = new List<GameDetail>();

            //Get the responses
            foreach (var request in requests)
            {
                var gameDetail = await request;

                if (gameDetail.Game == null) continue;

                gameDetails.Add(gameDetail);
            }

            return gameDetails;
        }

        private void LogException(string exception)
        {
            _logger.LogCritical(exception);
            _cancellationTokenSource.Cancel();

            Environment.ExitCode = 1;
        }
    }
}