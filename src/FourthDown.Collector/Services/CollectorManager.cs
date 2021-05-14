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
        Task<bool> RunAsync(CancellationToken cancellationToken);
    }

    public class CollectorManager : ICollectorManager
    {
        private readonly ILogger _logger;
        private readonly IGamePlayRepository _gamePlayRepository;
        private readonly ISqlGameRepository _sqlGameRepository;
        private readonly IGameRepository _gameRepository;
        private readonly IWriter _writer;

        private static readonly Channel<GameDetail> _channel = Channel.CreateUnbounded<GameDetail>();
        private ChannelReader<GameDetail> _channelReader = _channel.Reader;

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
        }

        public async Task<bool> RunAsync(CancellationToken cancellationToken)
        {
            /*
             * 1. Read games from github csv
             * 2. Check last games written in database
             * 3. Make request to get just those games
             * 4. Insert to database 
             */

            // List of gameIds from the database
            var lastGameDateTime = await _sqlGameRepository.GetLastGameDateTimeAsync(cancellationToken);

            // Games file with legacy games
            var games = await _gameRepository.GetGamesAsync(cancellationToken);

            var estTimeNow = DateTime.UtcNow;

            var gamesToWrite = games.Where(x =>
            {
                var gameTime = StringParser.EstDateTimeToUtc($"{x.Gameday.ToShortDateString()} {x.Gametime}");
                return gameTime > lastGameDateTime && gameTime < estTimeNow;
            }).ToList();

            if (!gamesToWrite.Any() || gamesToWrite.Count == 0)
            {
                return false;
            }

            await GetGamePlaysAsync(gamesToWrite, cancellationToken);

            await _writer.BulkInsertAsync(gamesToWrite, cancellationToken);

            return true;
        }

        private async Task GetGamePlaysAsync(IEnumerable<Game> games, CancellationToken cancellationToken)
        {
            var requestTasks = games
                .Select(game => _gamePlayRepository.GetGamePlaysAsync(game, cancellationToken))
                .ToList();

            //Wait for all the requests to finish
            await Task.WhenAll(requestTasks);

            //Get the responses
            foreach (var request in requestTasks)
            {
                var gameDetail = await request;

                if (gameDetail.Game == null)
                {
                    continue;
                }

                _channel.Writer.TryWrite(gameDetail);
            }
        }
    }
}