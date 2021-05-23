using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Shared.Extensions;
using FourthDown.Shared.Models;
using FourthDown.Shared.Utilities;
using OpenTracing;

namespace FourthDown.Shared.Repositories.Json
{
    public class JsonTeamRepository : ITeamRepository
    {
        private readonly ITracer _tracer;

        public JsonTeamRepository(ITracer tracer)
        {
            _tracer = tracer;
        }

        public async Task<IEnumerable<Team>> GetTeamsAsync(string webRootPath, CancellationToken cancellationToken)
        {
            using var scope = _tracer.BuildSpan(nameof(GetTeamsAsync)).StartActive(true);

            scope.LogStart(nameof(GetTeamsAsync));

            const string file = "teams.json";
            var filePath = Path.Join(webRootPath, "data", file);

            await using var sourceStream = File.Open(filePath, FileMode.Open);

            scope.LogEnd(nameof(GetTeamsAsync));

            var teams = await JsonSerializer.DeserializeAsync<IEnumerable<Team>>(
                sourceStream,
                StringParser.JsonSerializerOptions,
                cancellationToken);

            return teams
                .OrderBy(x => x.Conference)
                .ThenBy(x => x.DivisionIndex)
                .ThenBy(x => x.City);
        }
    }
}