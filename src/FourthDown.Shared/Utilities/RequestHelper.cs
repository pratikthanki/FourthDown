using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FourthDown.Shared.Utilities
{
    public interface IRequestHelper
    {
        Task<HttpResponseMessage> GetRequestResponse(string url, CancellationToken cancellationToken);
    }

    public class RequestHelper : IRequestHelper
    {
        private readonly HttpClient _httpClient;
        private readonly TimeSpan _timeout = TimeSpan.FromSeconds(10);
        private readonly HttpClientHandler _httpClientHandler = new HttpClientHandler
        {
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
        };

        public RequestHelper()
        {
            _httpClient = new HttpClient(_httpClientHandler) {Timeout = _timeout};
        }

        public async Task<HttpResponseMessage> GetRequestResponse(string url, CancellationToken cancellationToken)
        {
            return await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        }
    }
}