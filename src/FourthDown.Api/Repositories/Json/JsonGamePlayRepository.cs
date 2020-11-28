using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Api.Models;
using FourthDown.Api.Utilities;
using Microsoft.Extensions.Logging;

namespace FourthDown.Api.Repositories.Json
{
    public class JsonGamePlayRepository : IGamePlayRepository
    {
        private static ILogger<JsonGamePlayRepository> _logger;

        public JsonGamePlayRepository(ILogger<JsonGamePlayRepository> logger)
        {
            _logger = logger;
        }

        public async Task<GameDetail> GetGamePlaysAsync(
            string gameId,
            int season,
            CancellationToken cancellationToken)
        {
            var url = GetGameUrl(gameId, season);

            return await GetGameJson(url, cancellationToken);
        }

        private string GetGameUrl(string gameId, int season)
        {
            return $"https://github.com/pratikthanki/nflfastR-raw/blob/master/raw/{season}/{gameId}.json.gz?raw=true";
        }

        private static async Task<GameDetail> GetGameJson(string url, CancellationToken cancellationToken)
        {
            var response = await RequestHelper.GetRequestResponse(url, cancellationToken);
            var stream = await response.Content.ReadAsStreamAsync();

            var data = await ResponseHelper.ReadCompressedStreamToString(stream);

            var jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            GameRaw gameRaw = null;
            try
            {
                gameRaw = JsonSerializer.Deserialize<GameRaw>(data, jsonSerializerOptions);
            }
            catch (JsonException JsonException)
            {
                _logger.LogError($"Error in deserializing json string: {JsonException}\n url: {url}");
            }

            return gameRaw == null ? new GameDetail() : gameRaw.Data.Viewer.GameDetail;
        }
    }
}