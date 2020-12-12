using System.Collections.Generic;
using FourthDown.Api.Parameters;
using NUnit.Framework;

namespace FourthDown.Api.Tests
{
    [TestFixture]
    public class PlayByPlayQueryParameterTests
    {
        [Test]
        public void GetGameIdsBySeason_SplitIdsBySeason()
        {
            var parameters = new PlayByPlayQueryParameter()
            {
                GameId = "2020_01_ABC_XYZ,2020_02_DEF_XYZ"
            };

            var expected = new Dictionary<int, List<string>>()
            {
                {2020, new List<string>() {"2020_01_ABC_XYZ", "2020_02_DEF_XYZ"}}
            };

            Assert.AreEqual(expected, parameters.GetGameIdsBySeason());
        }
        
        [Test]
        public void GetGameIdsBySeason_GameIdsOverMultipleSeasons()
        {
            var parameters = new PlayByPlayQueryParameter()
            {
                GameId = "2019_01_ABC_XYZ,2020_02_DEF_XYZ"
            };

            var expected = new Dictionary<int, List<string>>()
            {
                {2019, new List<string>() {"2019_01_ABC_XYZ"}},
                {2020, new List<string>() {"2020_02_DEF_XYZ"}}
            };

            Assert.AreEqual(expected, parameters.GetGameIdsBySeason());
        }
        
        [Test]
        public void GetGameIdsBySeason_InvalidGameIdProvided()
        {
            var parameters = new PlayByPlayQueryParameter()
            {
                GameId = "2019_01_ABC_XZ,2020_02_DE_XYZ"
            };
            
            var actual = parameters.Validate();
            var message = "GameId's provided should be 15 characters long";

            Assert.AreEqual(message, actual["gameId"][0]);
        }
        
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