using FourthDown.Api.Parameters;
using NUnit.Framework;

namespace FourthDown.Api.Tests
{
    [TestFixture]
    public class PlayByPlayQueryParameterTests
    {
        [Test]
        public void GetGameIdsBySeason_InvalidRequestWhenAllFieldsProvided()
        {
            var parameters = new PlayByPlayQueryParameter()
            {
                Season = 2020,
                Week = 1,
                Team = "DAL",
                GameId = "2019_01_ABC_XYZ,2020_02_DE_XYZ"
            };

            var actual = parameters.Validate();
            var message = "If gameId is used then Week, Season and Team do not need to be provided";

            Assert.AreEqual(message, actual["query"][0]);
        }
    }
}