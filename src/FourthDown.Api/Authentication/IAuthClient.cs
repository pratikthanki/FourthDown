using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using FourthDown.Api.Configuration;
using FourthDown.Api.Utilities;
using Microsoft.Extensions.Options;

namespace FourthDown.Api.Authentication
{
    public interface IAuthClient
    {
        Task<ApiKey> Execute(string apiKey);
    }

    public class AuthClient : IAuthClient
    {
        private readonly AuthenticationOptions _authenticationOptions;

        public AuthClient(IOptions<AuthenticationOptions> apiKeyOptions)
        {
            _authenticationOptions = apiKeyOptions.Value;
        }

        public async Task<ApiKey> Execute(string apiKey)
        {
            Dictionary<string, ApiKey> apiKeys;
            if (_authenticationOptions.UseSampleAuth)
            {
                apiKeys = await JsonSerializer.DeserializeAsync<Dictionary<string, ApiKey>>(
                    File.OpenRead("Data/api-keys.json"),
                    StringParser.JsonSerializerOptions);
            }
            else
            {
                // TODO: get apiKeys from apikey management service
                apiKeys = new Dictionary<string, ApiKey>
                {
                    {"some-key", new ApiKey()}
                };
            }

            apiKeys.TryGetValue(apiKey, out var key);

            return key;
        }
    }
}