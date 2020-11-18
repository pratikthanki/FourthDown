using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Api.Models;
using FourthDown.Api.Utilities;

namespace FourthDown.Api.Repositories.Json
{
    public class JsonPlayByPlayRepository : IPlayByPlayRepository
    {
        private string GetGameUrl(string gameId, int season) =>
            $"https://github.com/pratikthanki/nflfastR-raw/blob/master/raw/{season}/{gameId}.json.gz?raw=true";

        public async Task<IEnumerable<GameRaw>> GetGamePlays(
            string gameId,
            int season,
            CancellationToken cancellationToken)
        {
            var url = GetGameUrl(gameId, season);

            return await GetGameJson(url, cancellationToken);
        }

        private static async Task<IEnumerable<GameRaw>> GetGameJson(string url, CancellationToken cancellationToken)
        {
            var response = await RequestHelper.GetRequestResponse(url, cancellationToken);
            var responseStream = await response.Content.ReadAsStreamAsync();

            return await JsonSerializer.DeserializeAsync<IEnumerable<GameRaw>>(
                responseStream,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }, cancellationToken);
        }
    }
}
