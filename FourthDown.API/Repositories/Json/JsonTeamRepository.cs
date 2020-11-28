using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;
using FourthDown.Api.Models;
using FourthDown.Api.Utilities;
using Microsoft.Extensions.Logging;
using OpenTracing;

namespace FourthDown.Api.Repositories.Json
{
    public class JsonTeamRepository : ITeamRepository
    {
        private readonly ILogger<JsonTeamRepository> _logger;
        private readonly ITracer _tracer;

        public JsonTeamRepository(
            ILogger<JsonTeamRepository> logger,
            ITracer tracer)
        {
            _logger = logger;
            _tracer = tracer;
        }

        public async IAsyncEnumerable<Team> GetTeamsAsync([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await foreach (var team in ReadTeamsJson(cancellationToken))
            {
                yield return team;
            }
        }

        private static async IAsyncEnumerable<Team> ReadTeamsJson(
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            const string file = "teams.json";
            var filePath = StringParser.GetDataFilePath(file);

            await using var SourceStream = File.Open(filePath, FileMode.Open);

            yield return await JsonSerializer.DeserializeAsync<Team>(
                SourceStream,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }, cancellationToken);
        }
    }
}