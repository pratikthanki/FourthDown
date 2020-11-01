using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using FourthDown.Shared.Models;

namespace FourthDown.API.Repositories.Json
{
    public class JsonPlayByPlayRepository : IPlayByPlayRepository
    {
        public IEnumerable<PlayByPlay> GetAllPlays()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<PlayByPlay> GetGamePlays(string gameId)
        {
            return ReadCsv(gameId).Where(x => x.GameId == gameId);
        }

        public PlayByPlay GetGamePlayByPlay(string gameId, string playId)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<PlayByPlay> GetTeamPlays(string gameId, string team)
        {
            throw new System.NotImplementedException();
        }

        private static IEnumerable<PlayByPlay> ReadCsv(string jsonFileName)
        {
            using var jsonFileReader = File.OpenText(jsonFileName);

            return JsonSerializer.Deserialize<PlayByPlay[]>(jsonFileReader.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }
    }
}