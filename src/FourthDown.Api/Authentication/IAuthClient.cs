using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using FourthDown.Api.Configuration;
using FourthDown.Api.Extensions;
using FourthDown.Api.Utilities;
using Microsoft.Extensions.Options;
using OpenTracing;

namespace FourthDown.Api.Authentication
{
    public interface IAuthClient
    {
        Task<ApiKey> CreateApiKey(string name);
        Task<ApiKey> GetApiKey(string apiKey);
    }

    public class AuthClient : IAuthClient
    {
        private readonly ITracer _tracer;
        private readonly ISlackClient _slackClient;
        private readonly AuthenticationOptions _authenticationOptions;

        public AuthClient(
            ITracer tracer,
            ISlackClient slackClient,
            IOptions<AuthenticationOptions> apiKeyOptions)
        {
            _slackClient = slackClient;
            _tracer = tracer;
            _authenticationOptions = apiKeyOptions.Value;
        }

        public async Task<ApiKey> GetApiKey(string apiKey)
        {
            using var scope = _tracer.BuildTrace(nameof(GetApiKey));
            
            scope.LogStart(nameof(GetApiKey));

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

            scope.LogEnd(nameof(GetApiKey));

            return apiKeys.FirstOrDefault(k => k.Key == apiKey);;
        }

        public async Task<ApiKey> CreateApiKey(string name)
        {
            using var scope = _tracer.BuildTrace(nameof(CreateApiKey));

            scope.LogStart(nameof(CreateApiKey));

            var apiKey = new ApiKey()
            {
                Name = name,
                Key = Guid.NewGuid().ToString(),
                CreationDateTime = DateTime.UtcNow,
                ExpirationDateTime = DateTime.UtcNow.AddDays(7)
            };
            
            scope.LogEnd(nameof(CreateApiKey));

            return await _slackClient.PostMessage(apiKey) ? apiKey : null;
        }
    }
}