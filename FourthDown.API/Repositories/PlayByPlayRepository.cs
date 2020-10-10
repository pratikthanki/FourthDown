using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using FourthDown.API.Models;

namespace FourthDown.API.Repositories
{
    public class PlayByPlayRepository : IPlayByPlayRepository
    {
        public IEnumerable<PlayByPlay> GetAllPlays()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<PlayByPlay> GetGamePlays(string gameId)
        {
            return ReadCsv(gameId);
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