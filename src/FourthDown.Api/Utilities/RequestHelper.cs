using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Serilog.Core;

namespace FourthDown.Api.Utilities
{
    public static class RequestHelper
    {
        public static async Task<HttpResponseMessage> GetRequestResponse<T>(
            string url,
            CancellationToken cancellationToken,
            ILogger<T> logger = default)
        {
            var timeout = TimeSpan.FromSeconds(10);
            var httpClientHandler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            var httpClient = new HttpClient(httpClientHandler) {Timeout = timeout};

            var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

            logger.LogInformation($"Fetching data. Url: {url}; Status: {response.StatusCode}");

            return response;
        }
    }
}