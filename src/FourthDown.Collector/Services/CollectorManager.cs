using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using FourthDown.Collector.Repositories;
using FourthDown.Shared.Models;
using FourthDown.Shared.Repositories;
using FourthDown.Shared.Utilities;
using Microsoft.Extensions.Logging;

namespace FourthDown.Collector.Services
{
    public interface ICollectorManager
    {
        Task<bool> TryGetGamesAsync(CancellationToken cancellationToken);
        Task ProcessGamesAsync(CancellationToken cancellationToken);
    }

    public class CollectorManager : ICollectorManager
    {
        private readonly ILogger _logger;
        private readonly IGamePlayRepository _gamePlayRepository;
        private readonly ISqlGameRepository _sqlGameRepository;
        private readonly IGameRepository _gameRepository;
        private readonly IWriter _writer;

        private readonly Channel<List<ApiGamePlay>> _channel;

        public CollectorManager(
            ILogger<CollectorManager> logger,
            IGamePlayRepository gamePlayRepository,
            ISqlGameRepository sqlGameRepository,
            IGameRepository gameRepository,
            IWriter writer)
        {
            _logger = logger;
            _gamePlayRepository = gamePlayRepository;
            _sqlGameRepository = sqlGameRepository;
            _gameRepository = gameRepository;
            _writer = writer;

            _channel = Channel.CreateUnbounded<List<ApiGamePlay>>();
        }

        public async Task<bool> TryGetGamesAsync(CancellationToken cancellationToken)
        {
            // List of gameIds from the database
            var lastGameDateTime = await _sqlGameRepository.GetLastGameDateTimeAsync(cancellationToken);

            // Games file with legacy games
            var legacyGames = (await _gameRepository.GetGamesAsync(cancellationToken)).ToList();

            var gamesToWrite = legacyGames.Where(x =>
            {
                var gameTime = StringParser.EstDateTimeToUtc($"{x.Gameday.ToShortDateString()} {x.Gametime}");
                return gameTime > lastGameDateTime && gameTime < DateTime.UtcNow;
            }).ToList();

            if (!gamesToWrite.Any())
            {
                _logger.LogInformation($"No games to write");
                return false;
            }

            _logger.LogInformation($"Games to write: {gamesToWrite.Select(x => x.GameId)}");

            await GetGamesToWrite(gamesToWrite, cancellationToken);
            
            _logger.LogInformation($"Writing game details to database");

            await _writer.BulkInsertGamesAsync(gamesToWrite, cancellationToken);

            return true;
        }

        public async Task ProcessGamesAsync(CancellationToken cancellationToken)
        {
            while (await _channel.Reader.WaitToReadAsync(cancellationToken))
            {
                while (_channel.Reader.TryRead(out var games))
                {
                    _logger.LogInformation($"Writing game play to database");

                    await _writer.BulkInsertGameDrivesAsync(games.Select(x => x.ToGameDrives()), cancellationToken);
                    await _writer.BulkInsertGamePlaysAsync(games.Select(x => x.ToGamePlays()), cancellationToken);
                    await _writer.BulkInsertGameScoringSummariesAsync(games.Select(x => x.ToGameScoringSummaries()), cancellationToken);
                }
            }
        }

        private async Task GetGamesToWrite(List<Game> gamesToWrite, CancellationToken cancellationToken)
        {
            var apiGames = new List<ApiGamePlay>();
            
            //Get the responses
            foreach (var game in gamesToWrite)
            {
                GameDetail gameDetail;

                try
                {
                    gameDetail = await _gamePlayRepository.GetGamePlaysAsync(game, cancellationToken);
                }
                catch (Exception e)
                {
                    _logger.LogCritical(e, $"Failed to get game: {game.GameId}");
                    throw;
                }

                if (gameDetail.Game == null)
                {
                    continue;
                }

                apiGames.Add(new ApiGamePlay(gameDetail));
            }
            
            _channel.Writer.TryWrite(apiGames);
        }
    }
}