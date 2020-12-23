using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Api.Extensions;
using FourthDown.Api.Models;
using FourthDown.Api.Utilities;
using Microsoft.Extensions.Logging;
using OpenTracing;

namespace FourthDown.Api.Repositories.Json
{
    public class JsonGamePlayRepository : IGamePlayRepository
    {
        private static ITracer _tracer;
        private static ILogger<JsonGamePlayRepository> _logger;

        public JsonGamePlayRepository(
            ITracer tracer,
            ILogger<JsonGamePlayRepository> logger)
        {
            _tracer = tracer;
            _logger = logger;
        }

        public async Task<GameDetail> GetGamePlaysAsync(Game game, CancellationToken cancellationToken)
        {
            using var scope = _tracer.BuildTrace(nameof(GetGamePlaysAsync));

            scope.LogStart(nameof(GetGamePlaysAsync));

            var url = GetGameUrl(game.GameId, game.Season);

            scope.LogEnd(nameof(GetGamePlaysAsync));

            var gameDetail = await GetGameJson(url, cancellationToken, scope);
            gameDetail.Game = game;

            return gameDetail;
        }

        public static string GetGameUrl(string gameId, int season)
        {
            // Games between 2001-2010 (inc) are in the raw_old folder
            var folder = season < 2011 ? "raw_old" : "raw";

            return $"{RepositoryEndpoints.GamePlayEndpoint}/{folder}/{season}/{gameId}.json.gz?raw=true";
        }

        private async Task<GameDetail> GetGameJson(string url, CancellationToken cancellationToken, IScope scope)
        {
            scope.LogStart(nameof(GetGameJson));

            var response = await RequestHelper.GetRequestResponse(url, cancellationToken, _logger);
            var stream = await response.Content.ReadAsStreamAsync();

            if (!response.IsSuccessStatusCode)
                return new GameDetail();

            var data = await ResponseHelper.ReadCompressedStreamToString(stream);

            scope.LogEnd(nameof(GetGameJson));

            return ParseResponseString(url, data);
        }

        private static GameDetail ParseResponseString(string url, string data)
        {
            GameRaw gameRaw = null;
            try
            {
                gameRaw = JsonSerializer.Deserialize<GameRaw>(data, StringParser.JsonSerializerOptions);
            }
            catch (JsonException JsonException)
            {
                _logger.LogError($"Error in deserializing json string: {JsonException}\n url: {url}");
            }

            return gameRaw == null ? new GameDetail() : gameRaw.Data.Viewer.GameDetail;
        }
    }
}