using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FourthDown.Api.Extensions;
using OpenTracing;

namespace FourthDown.Api.Authentication
{
    public interface ISlackClient
    {
        Task<bool> PostMessage(ApiKey apiKey);
        Task<IEnumerable<ApiKey>> ReadMessages();
    }

    public class SlackClient : ISlackClient
    {
        private readonly HttpClient _client;
        private readonly ITracer _tracer;

        public SlackClient(HttpClient client, ITracer tracer)
        {
            _client = client;
            _tracer = tracer;
        }

        public async Task<bool> PostMessage(ApiKey apiKey)
        {
            using var scope = _tracer.BuildTrace(nameof(PostMessage));
            
            const string _uri = @"/services/T01GR4LAZKQ/B01H6NFQQDR/TVaCALXy2ZstHBEg0L7JheSy";

            var text = $"```\n{{\n{apiKey}\n}}\n```";
            var contentObject = new {text = text};

            var contentObjectJson = JsonSerializer.Serialize(contentObject);
            var content = new StringContent(contentObjectJson, Encoding.UTF8, "application/json");

            scope.LogStart(nameof(PostMessage));

            var result = await _client.PostAsync(_uri, content);

            scope.LogEnd(nameof(PostMessage));

            return result.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<ApiKey>> ReadMessages()
        {
            using var scope = _tracer.BuildTrace(nameof(ReadMessages));

            const string botId = "B01H6NFQQDR";
            const string token = @"xoxb-1569156373670-1584021493538-IARGJ8wZvTESg37YoGYrVD2i";
            const string url = "https://slack.com/api/conversations.history?channel=C01H9GC8EEQ";

            string RemoveCharacters(string text)
            {
                return text
                    .Replace("```", "").Replace("'", "\"")
                    .Replace("\n", "").Replace("\t", "");
            }

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            scope.LogStart(nameof(ReadMessages));

            var response = await _client.GetAsync(url);
            var responseStream = await response.Content.ReadAsStreamAsync();

            var messages = await JsonSerializer.DeserializeAsync<Channel>(responseStream);

            var apiKeys = messages.Messages
                .Where(x => x.BotId == botId)
                .Select(x => JsonSerializer.Deserialize<ApiKey>(RemoveCharacters(x.Text)))
                .ToList();
            
            scope.LogEnd(nameof(ReadMessages));

            return apiKeys;
        }
    }

    public class Channel
    {
        [JsonPropertyName("messages")] public List<Message> Messages { get; set; }
    }

    public class Message
    {
        [JsonPropertyName("type")] public string Type { get; set; }
        [JsonPropertyName("subtype")] public string Subtype { get; set; }
        [JsonPropertyName("text")] public string Text { get; set; }
        [JsonPropertyName("ts")] public string Ts { get; set; }
        [JsonPropertyName("bot_id")] public string BotId { get; set; }
    }
}