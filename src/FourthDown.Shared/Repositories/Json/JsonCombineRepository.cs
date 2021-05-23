using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Shared.Extensions;
using FourthDown.Shared.Models;
using FourthDown.Shared.Utilities;
using OpenTracing;

namespace FourthDown.Shared.Repositories.Json
{
    public class JsonCombineRepository : ICombineRepository
    {
        private readonly ITracer _tracer;

        public JsonCombineRepository(ITracer tracer)
        {
            _tracer = tracer;
        }

        public async Task<IEnumerable<CombineWorkout>> GetCombineSummaryAsync(
            string webRootPath,
            int season,
            CancellationToken cancellationToken)
        {
            using var scope = _tracer.BuildSpan(nameof(GetCombineSummaryAsync)).StartActive(true);

            scope.LogStart(nameof(GetCombineSummaryAsync));

            var file = $"combine-{season}.json";
            var filePath = Path.Join(webRootPath, "data", file);

            await using var sourceStream = File.Open(filePath, FileMode.Open);

            scope.LogEnd(nameof(GetCombineSummaryAsync));

            var workouts = await JsonSerializer.DeserializeAsync<IEnumerable<CombineWorkout>>(
                sourceStream,
                StringParser.JsonSerializerOptions,
                cancellationToken);

            return workouts;
        }
    }
}