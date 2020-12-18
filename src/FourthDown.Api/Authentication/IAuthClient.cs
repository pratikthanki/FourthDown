using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using FourthDown.Api.Configuration;
using FourthDown.Api.Utilities;
using Microsoft.Extensions.Options;

namespace FourthDown.Api.Authentication
{
    public interface IAuthClient
    {
        Task<ApiKey> CreateApiKey(string name);
        Task<ApiKey> GetApiKey(string apiKey);
    }

    public class AuthClient : IAuthClient
    {
        private readonly ISlackClient _slackClient;
        private readonly AuthenticationOptions _authenticationOptions;

        public AuthClient(
            ISlackClient slackClient,
            IOptions<AuthenticationOptions> apiKeyOptions)
        {
            _slackClient = slackClient;
            _authenticationOptions = apiKeyOptions.Value;
        }

        public async Task<ApiKey> GetApiKey(string apiKey)
        {
            IEnumerable<ApiKey> apiKeys;
            if (_authenticationOptions.UseSampleAuth)
            {
                apiKeys = await JsonSerializer.DeserializeAsync<List<ApiKey>>(
                    File.OpenRead("Data/api-keys.json"),
                    StringParser.JsonSerializerOptions);
            }
            else
            {
                apiKeys = await _slackClient.ReadMessages();
            }

            return apiKeys.FirstOrDefault(k => k.Key == apiKey);;
        }

        public async Task<ApiKey> CreateApiKey(string name)
        {
            var apiKey = new ApiKey()
            {
                Name = name,
                Key = Guid.NewGuid().ToString(),
                CreationDateTime = DateTime.UtcNow,
                ExpirationDateTime = DateTime.UtcNow.AddDays(7)
            };
            
            return await _slackClient.PostMessage(apiKey) ? apiKey : null;
        }
    }
}