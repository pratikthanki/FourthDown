using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FourthDown.Shared.Utilities
{
    public static class RequestHelper
    {
        public static async Task<HttpResponseMessage> GetRequestResponse(
            string url,
            CancellationToken cancellationToken)
        {
            var timeout = TimeSpan.FromSeconds(10);
            var httpClientHandler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            var httpClient = new HttpClient(httpClientHandler) {Timeout = timeout};

            var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

            return response;
        }
    }
}