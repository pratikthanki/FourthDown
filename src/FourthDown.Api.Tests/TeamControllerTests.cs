using System.Net;
using System.Threading.Tasks;
using FourthDown.Api.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace FourthDown.Api.Tests
{
    [TestFixture]
    public class TeamControllerTests : FakeApi
    {
        private const string endpoint = "/api/teams";
        private const string validApiKey = "api-key";
        private const string invalidApiKey = "invalid-api-key";

        [Test]
        public async Task Get_Should_Retrieve_Teams()
        {
            var (statusCode, response) = await SendRequest(endpoint, validApiKey, null);
            Assert.AreEqual(HttpStatusCode.OK, statusCode);
        }
        
        [Test]
        public async Task Get_WithInvalidApiKey_Fails()
        {
            var (statusCode, response) = await SendRequest(endpoint, invalidApiKey, null);
            Assert.AreEqual(HttpStatusCode.Unauthorized, statusCode);
        }
    }
}