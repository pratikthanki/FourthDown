using System.IO;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.GZip;

namespace FourthDown.Api.Utilities
{
    public class ResponseHelper
    {
        public static async Task<string> ReadCompressedStreamToString(Stream stream)
        {
            await using var inStream = new GZipInputStream(stream);
            await using var MemoryStream = new MemoryStream();

            var buffer = new byte[4096];
            StreamUtils.Copy(inStream, MemoryStream, buffer);

            var data = Encoding.UTF8.GetString(MemoryStream.ToArray());

            return data;
        }
    }
}