using FourthDown.Api.Parameters;
using NUnit.Framework;

namespace FourthDown.Api.Tests
{
    [TestFixture]
    public class QueryParameterTests
    {
        [Test]
        public void PlayByPlayQueryParameter_InvalidMultiplePassed()
        {
            var parameters = new PlayByPlayQueryParameter()
            {
                Season = 2020,
                Week = 1,
                Team = "DAL",
                GameId = "2019_01_ABC_XYZ,2020_02_DE_XYZ"
            };

            var errors = parameters.Validate();

            Assert.AreEqual(0, errors.Count);
        }

        [Test]
        public void PlayByPlayQueryParameter_InvalidSeason()
        {
            var parameters = new PlayByPlayQueryParameter()
            {
                Season = 2099
            };

            var errors = parameters.Validate();

            Assert.AreEqual(1, errors.Count);
        }

        [Test]
        public void ScheduleQueryParameter_InvalidWeekNumber()
        {
            var parameters = new ScheduleQueryParameter()
            {
                Week = 99
            };

            var errors = parameters.Validate();

            Assert.AreEqual(1, errors.Count);
        }
        
        [Test]
        public void PlayByPlayQueryParameter_NoParameters()
        {
            var parameters = new PlayByPlayQueryParameter();
            var errors = parameters.Validate();

            Assert.AreEqual(1, errors.Count);
        }
    }
}