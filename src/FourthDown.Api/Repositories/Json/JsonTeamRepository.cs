using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Api.Extensions;
using FourthDown.Api.Models;
using Microsoft.AspNetCore.Hosting;
using OpenTracing;

namespace FourthDown.Api.Repositories.Json
{
    public class JsonTeamRepository : ITeamRepository
    {
        private readonly ITracer _tracer;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public JsonTeamRepository(ITracer tracer, IWebHostEnvironment webHostEnvironment)
        {
            _tracer = tracer;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IEnumerable<Team>> GetTeamsAsync(CancellationToken cancellationToken)
        {
            using var scope = _tracer.BuildSpan(nameof(GetTeamsAsync)).StartActive(true);
            
            return await ReadTeamsJson(cancellationToken, scope);
        }

        private async Task<IEnumerable<Team>> ReadTeamsJson(
            CancellationToken cancellationToken, IScope scope)
        {
            scope.LogStart(nameof(ReadTeamsJson));

            const string file = "teams.json";
            var filePath = Path.Join(_webHostEnvironment.WebRootPath, "data", file);

            await using var SourceStream = File.Open(filePath, FileMode.Open);
            
            scope.LogEnd(nameof(ReadTeamsJson));
            
            return await JsonSerializer.DeserializeAsync<IEnumerable<Team>>(
                SourceStream,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }, cancellationToken);
        }
    }
}