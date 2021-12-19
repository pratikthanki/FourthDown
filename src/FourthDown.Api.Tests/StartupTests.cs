using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;

namespace FourthDown.Api.Tests
{
    public class StartupTests
    {
        private static WebApplicationFactory<Startup> _factory;

        [SetUp]
        public void SetUp()
        {
            _factory = new WebApplicationFactory<Startup>();
        }

        [Test]
        public void AppDoesNotThrowWhenSampleAppOptionNotSet()
        {
            Assert.DoesNotThrow(() => { _factory.WithWebHostBuilder(builder => { }).CreateClient(); });
        }

        [TearDown]
        public void TearDown()
        {
            _factory.Dispose();
        }
    }
}