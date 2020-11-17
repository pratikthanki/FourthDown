using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using FourthDown.Api.Models;
using FourthDown.Api.Utilities;

namespace FourthDown.Api.Repositories.Json
{
    public class JsonTeamRepository : ITeamRepository
    {
        public JsonTeamRepository()
        {
        }

        public async Task<IEnumerable<Team>> GetTeams()
        {
            return await ReadTeamsJson();
        }

        private static async Task<IEnumerable<Team>> ReadTeamsJson()
        {
            const string file = "teams.json";
            var filePath = StringParser.GetDataFilePath(file);

            await using var SourceStream = File.Open(filePath, FileMode.Open);

            return await JsonSerializer.DeserializeAsync<IEnumerable<Team>>(
                SourceStream,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }
    }
}