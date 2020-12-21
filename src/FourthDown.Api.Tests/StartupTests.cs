using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace FourthDown.Api.Tests
{
    public class StartupTests
    {
        private static void BuildWith(Dictionary<string, string> appConfig)
        {
            var factory = new WebApplicationFactory<Startup>();
            factory.WithWebHostBuilder(builder =>
            {
                builder
                    .ConfigureAppConfiguration((_, config) =>
                    {
                        config.Sources.Clear();
                        config.AddInMemoryCollection(appConfig);
                    });
            }).CreateClient();
        }

        [Test,
         TestCase(null)]
        public void AppDoesNotThrowWhenSampleAppOptionNotSet(string option)
        {
            Assert.DoesNotThrow(() =>
            {
                BuildWith(new Dictionary<string, string>
                {
                    ["UseSampleAuth"] = option
                });
            });
        }
    }
}