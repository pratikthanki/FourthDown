using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Api.Extensions;
using FourthDown.Api.Models;
using FourthDown.Api.Utilities;
using Microsoft.AspNetCore.Hosting;
using OpenTracing;

namespace FourthDown.Api.Repositories.Json
{
    public class JsonCombineRepository : ICombineRepository
    {
        private readonly ITracer _tracer;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public JsonCombineRepository(ITracer tracer, IWebHostEnvironment webHostEnvironment)
        {
            _tracer = tracer;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IEnumerable<CombineWorkout>> GetCombineSummaryAsync(
            int season,
            CancellationToken cancellationToken)
        {
            using var scope = _tracer.BuildSpan(nameof(GetCombineSummaryAsync)).StartActive(true);

            scope.LogStart(nameof(GetCombineSummaryAsync));

            var file = $"combine-{season}.json";
            var filePath = Path.Join(_webHostEnvironment.WebRootPath, "data", file);

            await using var SourceStream = File.Open(filePath, FileMode.Open);

            scope.LogEnd(nameof(GetCombineSummaryAsync));

            var workouts = await JsonSerializer.DeserializeAsync<IEnumerable<CombineWorkout>>(
                SourceStream,
                StringParser.JsonSerializerOptions,
                cancellationToken);

            return workouts;
        }
    }
}