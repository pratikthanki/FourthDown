using FourthDown.Api.Repositories.Json;
using FourthDown.Api.Utilities;
using NUnit.Framework;

namespace FourthDown.Api.Tests
{
    public class EndpointTests
    {
        [Test]
        public void GameRepositoryEndpoint()
        {
            const string expected = "https://github.com/leesharpe/nfldata/blob/master/data/games.csv?raw=true";

            Assert.AreEqual(expected, RepositoryEndpoints.GamesEndpoint);
        }

        [Test]
        public void PlayByPlayRepositoryEndpoint()
        {
            const string expected =
                "https://github.com/pratikthanki/nflfastR-data/blob/master/data/play_by_play_2020.csv.gz?raw=true";

            var path = $"{RepositoryEndpoints.PlayByPlayEndpoint}/play_by_play_{2020}.csv.gz?raw=true";

            Assert.AreEqual(expected, path);
        }

        [Test]
        public void GamePlayRepositoryEndpoint()
        {
            const int season = 2020;
            const string gameId = "2020_01_ABC_XYZ";
            var expected =
                $"https://github.com/pratikthanki/nflfastR-raw/blob/upstream/raw/{season}/{gameId}.json.gz?raw=true";

            var actual = JsonGamePlayRepository.GetGameUrl(gameId, season);
            
            Assert.AreEqual(expected, actual);
        }
    }
}