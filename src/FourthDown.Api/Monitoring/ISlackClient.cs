using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Api.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FourthDown.Api.Monitoring
{
    public interface ISlackClient
    {
        Task PostMessage(HttpContext httpContext);
    }

    public class SlackClient : ISlackClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<SlackClient> _logger;
        private readonly IOptions<MonitoringOptions> _options;

        public SlackClient(
            HttpClient client,
            ILogger<SlackClient> logger,
            IOptions<MonitoringOptions> options)
        {
            _client = client;
            _logger = logger;
            _options = options;
        }

        public async Task PostMessage(HttpContext httpContext)
        {
            if (!_options.Value.EnableSlackStreaming)
                return;

            const string _uri = @"/services/T01GR4LAZKQ/B01H6NFQQDR/TVaCALXy2ZstHBEg0L7JheSy";

            var ip = httpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            var ipApi = await GetIp(ip);
            var requestLog = new RequestLog()
            {
                Endpoint = httpContext.GetEndpoint().DisplayName,
                Method = httpContext.Request.Method,
                country = ipApi.country,
                city = ipApi.city,
                lat = ipApi.lat,
                lon = ipApi.lon,
                timezone = ipApi.timezone

            };

            var contentObject = new {text = $"```\n{{\n{requestLog}\n}}\n```"};

            var contentObjectJson = JsonSerializer.Serialize(contentObject);
            var content = new StringContent(contentObjectJson, Encoding.UTF8, "application/json");

            await _client.PostAsync(_uri, content);
        }

        private async Task<IpApi> GetIp(string ip)
        {
            var url = $"http://ip-api.com/json/{ip}";
            var response = await RequestHelper.GetRequestResponse(url, CancellationToken.None);

            var responseBody = await response.Content.ReadAsStreamAsync();

            return await JsonSerializer.DeserializeAsync<IpApi>(responseBody);
        }
    }

    public class MonitoringOptions
    {
        public bool EnableSlackStreaming { get; set; }
    }

    public class RequestLog : IpApi
    {
        public string Endpoint { get; set; }
        public string Method { get; set; }

        public override string ToString()
        {
            return $"\n\t'{nameof(Endpoint)}': '{Endpoint}', " +
                   $"\n\t'{nameof(Method)}': '{Method}', " +
                   $"\n\t'{nameof(country)}': '{country}', " +
                   $"\n\t'{nameof(city)}': '{city}', " +
                   $"\n\t'{nameof(lat)}': '{lat}', " +
                   $"\n\t'{nameof(lon)}': '{lon}', " +
                   $"\n\t'{nameof(timezone)}': '{timezone}'";
        }
    }

    public class IpApi
    {
        public string query { get; set; }
        public string status { get; set; }
        public string country { get; set; }
        public string countryCode { get; set; }
        public string region { get; set; }
        public string regionName { get; set; }
        public string city { get; set; }
        public string zip { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public string timezone { get; set; }
        public string isp { get; set; }
        public string org { get; set; }
        public string @as { get; set; }
    }
}