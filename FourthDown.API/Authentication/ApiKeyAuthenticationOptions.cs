using Microsoft.AspNetCore.Authentication;

namespace FourthDown.Api.Authentication
{
    public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "API Key";
        public static string Scheme = DefaultScheme;
        public const string AuthenticationType = DefaultScheme;
    }
}