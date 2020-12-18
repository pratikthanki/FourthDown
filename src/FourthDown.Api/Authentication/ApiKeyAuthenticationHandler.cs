using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace FourthDown.Api.Authentication
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
    {
        private const string ProblemDetailsContentType = "application/problem+json";
        private const string ApiKeyHeaderName = "X-Api-Key";

        private readonly IAuthClient _authClient;

        public ApiKeyAuthenticationHandler(
            IOptionsMonitor<ApiKeyAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IAuthClient authClient) : base(options, logger, encoder, clock)
        {
            _authClient = authClient;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(ApiKeyHeaderName, out var apiKeyHeaderValues))
                return AuthenticateResult.NoResult();

            var providedApiKey = apiKeyHeaderValues.FirstOrDefault();

            if (apiKeyHeaderValues.Count == 0 || string.IsNullOrWhiteSpace(providedApiKey))
                return AuthenticateResult.NoResult();

            var existingApiKey = await _authClient.GetApiKey(providedApiKey);

            if (existingApiKey == null)
                return AuthenticateResult.Fail("Invalid API Key provided.");

            if (DateTime.UtcNow.Date > existingApiKey.ExpirationDateTime)
                return AuthenticateResult.Fail($"Provided API Key has expired: {existingApiKey.ExpirationDateTime}.");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, existingApiKey.Name),
                new Claim(ClaimTypes.Expiration,
                    existingApiKey.ExpirationDateTime.ToString(CultureInfo.InvariantCulture))
            };

            var identity = new ClaimsIdentity(claims, ApiKeyAuthenticationOptions.AuthenticationType);
            var identities = new List<ClaimsIdentity> {identity};
            var principal = new ClaimsPrincipal(identities);
            var ticket = new AuthenticationTicket(principal, ApiKeyAuthenticationOptions.Scheme);

            return AuthenticateResult.Success(ticket);
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = (int) HttpStatusCode.Unauthorized;
            Response.ContentType = ProblemDetailsContentType;

            await base.HandleChallengeAsync(properties);

            var message = new Dictionary<string, string>
            {
                {"Title", HttpStatusCode.Unauthorized.ToString()},
                {"Status", ((int) HttpStatusCode.Unauthorized).ToString()},
                {
                    "Detail",
                    "Provided API key is either invalid or has expired. Use the POST '/api/auth' endpoint to create an API Key."
                }
            };

            var text = JsonConvert.SerializeObject(message);

            var errorResponse = Encoding.UTF8.GetBytes(text);
            await Response.Body.WriteAsync(errorResponse);
        }
    }

    public class ApiKey
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public DateTime CreationDateTime { get; set; }
        public DateTime ExpirationDateTime { get; set; }

        public override string ToString()
        {
            const string datePattern = "yyyy-MM-ddTHH:mm:ss";

            return $"\n\t'{nameof(Name)}': '{Name}', " +
                   $"\n\t'{nameof(Key)}': '{Key}', " +
                   $"\n\t'{nameof(CreationDateTime)}': '{CreationDateTime.ToString(datePattern)}', " +
                   $"\n\t'{nameof(ExpirationDateTime)}': '{ExpirationDateTime.ToString(datePattern)}'";
        }
    }
}