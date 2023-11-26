using System.Collections.Concurrent;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Shared.Extensions;
using FourthDown.Shared.Models;
using FourthDown.Shared.Utilities;
using Microsoft.Extensions.Logging;
using OpenTracing;

namespace FourthDown.Shared.Repositories.Json
{
    public class JsonGamePlayRepository : IGamePlayRepository
    {
        private static ITracer _tracer;
        private static ILogger<JsonGamePlayRepository> _logger;
        private readonly IRequestHelper _requestHelper;
        private readonly ConcurrentDictionary<Game, GameDetail> _gamesCache = new ConcurrentDictionary<Game, GameDetail>();

        public JsonGamePlayRepository(
            ITracer tracer,
            ILogger<JsonGamePlayRepository> logger,
            IRequestHelper requestHelper)
        {
            _tracer = tracer;
            _logger = logger;
            _requestHelper = requestHelper;
        }

        public async Task<GameDetail> GetGamePlaysAsync(Game game, CancellationToken cancellationToken)
        {
            using var scope = _tracer.BuildTrace(nameof(GetGamePlaysAsync));

            if (_gamesCache.TryGetValue(game, out var gameDetail))
            {
                _logger.LogInformation($"Game found in cache: {game.GameId}");
                return gameDetail;
            }

            scope.LogStart(nameof(GetGamePlaysAsync));

            var url = GetGameUrl(game.GameId, game.Season);
            gameDetail = await GetGameJson(url, cancellationToken, scope);
            gameDetail.Game = game;

            _gamesCache[game] = gameDetail;

            scope.LogEnd(nameof(GetGamePlaysAsync));

            return gameDetail;
        }

        public static string GetGameUrl(string gameId, int season)
        {
            return $"{RepositoryEndpoints.GamePlayEndpoint}/{season}/{gameId}.json.gz?raw=true";
        }

        private async Task<GameDetail> GetGameJson(string url, CancellationToken cancellationToken, IScope scope = null)
        {
            scope?.LogStart(nameof(GetGameJson));

            var response = await _requestHelper.GetRequestResponse(url, cancellationToken);

            _logger.LogInformation($"Fetching data. Url: {url}; Status: {response.StatusCode}");

            if (!response.IsSuccessStatusCode)
                return new GameDetail();

            var data = ResponseHelper.ReadCompressedStreamToString(response);

            scope?.LogEnd(nameof(GetGameJson));

            return await ParseResponseString(url, data);
        }

        private static async Task<GameDetail> ParseResponseString(string url, Task<string> dataTask)
        {
            GameRaw gameRaw = null;
            var data = await dataTask;
            try
            {
                gameRaw = JsonSerializer.Deserialize<GameRaw>(data, StringParser.JsonSerializerOptions);
            }
            catch (JsonException jsonException)
            {
                _logger.LogError($"Error in deserializing json string: {jsonException}\n url: {url}");
            }

            return gameRaw == null ? new GameDetail() : gameRaw.Data.Viewer.GameDetail;
        }
    }
}