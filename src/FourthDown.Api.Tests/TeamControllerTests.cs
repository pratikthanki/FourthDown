using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;

namespace FourthDown.Api.Tests
{
    [TestFixture]
    public class TeamControllerTests : FakeApi
    {
        private const string endpoint = "/api/teams";

        [Test]
        public async Task Get_Should_Retrieve_Teams()
        {
            var (statusCode, response) = await SendRequest(endpoint, null);
            Assert.AreEqual(HttpStatusCode.OK, statusCode);
        }
    }
}