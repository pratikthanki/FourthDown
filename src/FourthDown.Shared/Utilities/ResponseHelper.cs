using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;

namespace FourthDown.Shared.Utilities
{
    public static class ResponseHelper
    {
        public static async Task<string> ReadCompressedStreamToString(HttpResponseMessage response)
        {
            var inputBytes = await response.Content.ReadAsByteArrayAsync();

            await using var inputStream = new MemoryStream(inputBytes);
            await using var gZipStream = new GZipStream(inputStream, CompressionMode.Decompress);
            using var streamReader = new StreamReader(gZipStream);

            return await streamReader.ReadToEndAsync();
        }
    }
}