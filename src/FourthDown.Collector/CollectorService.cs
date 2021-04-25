using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Collector.Repositories;
using FourthDown.Shared.Models;
using FourthDown.Shared.Repositories;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FourthDown.Collector
{
    public class CollectorService : BackgroundService
    {
        private readonly ILogger<CollectorService> _logger;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly CancellationToken _cancellationToken;

        private readonly ISqlGameRepository _sqlGameRepository;
        private readonly IGameRepository _gameRepository;
        private readonly IGamePlayRepository _gamePlayRepository;

        public CollectorService(
            ILogger<CollectorService> logger,
            IHostApplicationLifetime appLifetime,
            ISqlGameRepository sqlGameRepository,
            IGameRepository gameRepository,
            IGamePlayRepository gamePlayRepository)
        {
            _logger = logger;
            _sqlGameRepository = sqlGameRepository;
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

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            /*
             * 1. Read games from github csv
             * 2. Check last games written in database
             * 3. Make request to get just those games
             * 4. Insert to database 
             */

            // List of gameIds from the database
            var gameIdsWritten = await _sqlGameRepository.GetGameIdsAsync(_cancellationToken);

            // Games file with legacy games
            var games = await _gameRepository.GetGamesAsync(_cancellationToken);
            var gamesToWrite = games.Where(x => !gameIdsWritten.Contains(x.GameId));

            var gameDetails = await GetGamePlays(gamesToWrite, _cancellationToken);

            // _databaseClient.Write();

            try
            {
                // do something
            }
            catch (Exception exception)
            {
                _logger.LogCritical(exception.Message);
                _cancellationTokenSource.Cancel();

                Environment.ExitCode = 1;
            }

            _cancellationTokenSource.Cancel();

            Environment.ExitCode = 0;
        }

        private async Task<IEnumerable<GameDetail>> GetGamePlays(
            IEnumerable<Game> games, 
            CancellationToken cancellationToken)
        {
            var requestTasks = games
                .Select(game => _gamePlayRepository.GetGamePlaysAsync(game, cancellationToken))
                .ToList();

            //Wait for all the requests to finish
            await Task.WhenAll(requestTasks);

            var gameDetails = new List<GameDetail>();

            //Get the responses
            foreach (var request in requestTasks)
            {
                var gameDetail = await request;

                if (gameDetail.Game == null) continue;

                gameDetails.Add(gameDetail);
            }

            return gameDetails;
        }
    }
}