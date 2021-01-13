using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace FourthDown.Api.Utilities
{
    public class ResponseHelper
    {
        public static Task<string> ReadCompressedStreamToString(byte[] inputBytes)
        {
            using var inputStream = new MemoryStream(inputBytes);
            using var gZipStream = new GZipStream(inputStream, CompressionMode.Decompress);
            using var streamReader = new StreamReader(gZipStream);

            var decompressed = streamReader.ReadToEndAsync();
            return decompressed;
        }
    }
}