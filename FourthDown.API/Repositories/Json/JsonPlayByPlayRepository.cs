using System.IO;
using System.IO.Compression;
using System.Text;
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

        public async Task<GameDetail> GetGamePlays(
            string gameId,
            int season,
            CancellationToken cancellationToken)
        {
            var url = GetGameUrl(gameId, season);

            return await GetGameJson(url, cancellationToken);
        }

        private static async Task<GameDetail> GetGameJson(string url, CancellationToken cancellationToken)
        {
            var response = await RequestHelper.GetRequestResponse(url, cancellationToken);
            var stream = await response.Content.ReadAsStreamAsync();

            string data;
            await using (var gs = new GZipStream(stream, CompressionMode.Decompress))
            await using (var mso = new MemoryStream())
            {
                var bytes = new byte[4096];
                int cnt;
                while ((cnt = await gs.ReadAsync(bytes, 0, bytes.Length, cancellationToken)) != 0)
                {
                    await mso.WriteAsync(bytes, 0, cnt, cancellationToken);
                }

                data = Encoding.UTF8.GetString(mso.ToArray());
            }

            var jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            
            var gameRaw = JsonSerializer.Deserialize<GameRaw>(data, jsonSerializerOptions);

            return gameRaw.Data.Viewer.GameDetail;
        }
    }
}
