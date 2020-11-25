using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
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

        public async Task<IEnumerable<Team>> GetTeams(CancellationToken cancellationToken)
        {
            return await ReadTeamsJson(cancellationToken);
        }

        private static async Task<IEnumerable<Team>> ReadTeamsJson(CancellationToken cancellationToken)
        {
            const string file = "teams.json";
            var filePath = StringParser.GetDataFilePath(file);

            await using var SourceStream = File.Open(filePath, FileMode.Open);

            return await JsonSerializer.DeserializeAsync<IEnumerable<Team>>(
                SourceStream,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }, cancellationToken);
        }
    }
}