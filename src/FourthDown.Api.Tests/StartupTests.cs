using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;

namespace FourthDown.Api.Tests
{
    public class StartupTests
    {
        private static void Build()
        {
            var factory = new WebApplicationFactory<Startup>();
            factory.WithWebHostBuilder(builder =>
            {
            }).CreateClient();
        }

        [Test,
         TestCase(null)]
        public void AppDoesNotThrowWhenSampleAppOptionNotSet(string option)
        {
            Assert.DoesNotThrow(Build);
        }
    }
}