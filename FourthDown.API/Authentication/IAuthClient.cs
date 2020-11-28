using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using FourthDown.Api.Configuration;
using Microsoft.Extensions.Options;

namespace FourthDown.Api.Authentication
{
    public interface IAuthClient
    {
        Task<ApiKey> Execute(string apiKey);
    }

    public class AuthClient : IAuthClient
    {
        private static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        private readonly ApiKeyOptions _apiKeyOptions;

        public AuthClient(IOptions<ApiKeyOptions> apiKeyOptions)
        {
            _apiKeyOptions = apiKeyOptions.Value;
        }

        public async Task<ApiKey> Execute(string apiKey)
        {
            Dictionary<string, ApiKey> apiKeys;
            if (!_apiKeyOptions.UseSampleAuth)
                // TODO: get apiKeys from apikey management service
                apiKeys = new Dictionary<string, ApiKey>
                {
                    {"some-key", new ApiKey()}
                };
            else
                apiKeys = await JsonSerializer.DeserializeAsync<Dictionary<string, ApiKey>>(
                    File.OpenRead("Data/api-keys.json"), SerializerOptions);

            apiKeys.TryGetValue(apiKey, out var key);

            return key;
        }
    }
}