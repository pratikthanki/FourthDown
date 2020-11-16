using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FourthDown.Api.Utilities
{
    public static class WebClient
    {
        public static async Task<HttpResponseMessage> CreateRequest(string url, CancellationToken cancellationToken)
        {
            var timeout = TimeSpan.FromSeconds(10);
            var httpClientHandler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            var httpClient = new HttpClient(httpClientHandler) {Timeout = timeout};
            var response = await httpClient.GetAsync(url, cancellationToken);

            return response;
        }
    }
}