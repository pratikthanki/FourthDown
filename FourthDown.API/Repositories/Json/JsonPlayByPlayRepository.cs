using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Api.Models;
using FourthDown.Api.Utilities;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.GZip;
using Microsoft.Extensions.Logging;

namespace FourthDown.Api.Repositories.Json
{
    public class JsonPlayByPlayRepository : IPlayByPlayRepository
    {
        private static ILogger<JsonPlayByPlayRepository> _logger;
        
        public JsonPlayByPlayRepository(ILogger<JsonPlayByPlayRepository> logger)
        {
            _logger = logger;
        }

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

            var data = await ReadCompressedStreamToString(stream);

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

            return gameRaw.Data.Viewer.GameDetail;
        }

        private static async Task<string> ReadCompressedStreamToString(Stream stream)
        {
            await using var inStream = new GZipInputStream(stream);
            await using var MemoryStream = new MemoryStream();

            byte[] buffer = new byte[4096];
            StreamUtils.Copy(inStream, MemoryStream, buffer);
            
            var data = Encoding.UTF8.GetString(MemoryStream.ToArray());

            return data;
        }
    }
}
