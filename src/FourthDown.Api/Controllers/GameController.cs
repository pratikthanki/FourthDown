using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Api.Extensions;
using FourthDown.Api.Models;
using FourthDown.Api.Monitoring;
using FourthDown.Api.Parameters;
using FourthDown.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenTracing;

namespace FourthDown.Api.Controllers
{
    [Route("api/game")]
    [ApiVersion("1.0")]
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status404NotFound)]
    public class GameController : ControllerBase
    {
        private readonly ITracer _tracer;
        private readonly IGamePlayService _pbpService;

        public GameController(ITracer tracer, IGamePlayService pbpService)
        {
            _tracer = tracer;
            _pbpService = pbpService;
        }

        /// <summary>
        ///     Play by Play data for a set of games
        ///     Either GameId or a combination of Season, Week and Team should be provided
        /// </summary>
        /// <param name="queryParameter">Combination of Season, Week and Team</param>
        /// <param name="cancellationToken"></param>
        /// <returns>List of game play by plays</returns>
        [HttpGet("plays")]
        public async IAsyncEnumerable<GamePlays> GetPlays(
            [FromQuery] PlayByPlayQueryParameter queryParameter,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            using var scope = _tracer.InitializeTrace(HttpContext, nameof(GetPlays));

            scope.LogStart(nameof(_pbpService.GetGamePlaysAsync));

            int count = 0;
            await foreach (var play in _pbpService.GetGamePlaysAsync(queryParameter, cancellationToken))
            {
                ++count;
                yield return play;
            }

            scope.LogEnd(nameof(_pbpService.GetGamePlaysAsync));

            MetricCollector.RegisterMetrics(HttpContext, Request, count);
        }

        /// <summary>
        ///     Game drives summarised from plays
        ///     Either GameId or a combination of Season, Week and Team should be provided
        /// </summary>
        /// <param name="queryParameter">Combination of Season, Week and Team</param>
        /// <param name="cancellationToken"></param>
        /// <returns>List of game drives</returns>
        [HttpGet("drives")]
        public async IAsyncEnumerable<GameDrives> GetDrives(
            [FromQuery] PlayByPlayQueryParameter queryParameter,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            // var errors = queryParameter.Validate();
            // if (errors.Count > 0)
            //     return BadRequest(new ValidationProblemDetails(errors)
            //     {
            //         Title = "There are errors with your request.",
            //         Status = StatusCodes.Status400BadRequest
            //     });

            using var scope = _tracer.InitializeTrace(HttpContext, nameof(GetPlays));

            scope.LogStart(nameof(_pbpService.GetGameDrivesAsync));

            int count = 0;
            await foreach (var play in _pbpService.GetGameDrivesAsync(queryParameter, cancellationToken))
            {
                ++count;
                yield return play;
            }

            scope.LogEnd(nameof(_pbpService.GetGameDrivesAsync));

            MetricCollector.RegisterMetrics(HttpContext, Request, count);

            // if (plays == null || !plays.Any())
            //     return NotFound(new ValidationProblemDetails(queryParameter.ToKeyValues())
            //     {
            //         Title = "No data for the request parameters given.",
            //         Status = StatusCodes.Status404NotFound
            //     });

        }

        /// <summary>
        ///     List of scoring drives with updated team scores
        ///     Either GameId or a combination of Season, Week and Team should be provided
        /// </summary>
        /// <param name="queryParameter">Combination of Season, Week and Team</param>
        /// <param name="cancellationToken"></param>
        /// <returns>List of game scoring summaries</returns>
        [HttpGet("scoringsummaries")]
        public async IAsyncEnumerable<GameScoringSummaries> GetScoringSummaries(
            [FromQuery] PlayByPlayQueryParameter queryParameter,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            using var scope = _tracer.InitializeTrace(HttpContext, nameof(GetPlays));

            scope.LogStart(nameof(_pbpService.GetGameScoringSummariesAsync));

            int count = 0;
            await foreach (var play in _pbpService.GetGameScoringSummariesAsync(queryParameter, cancellationToken))
            {
                ++count;
                yield return play;
            }

            scope.LogEnd(nameof(_pbpService.GetGameScoringSummariesAsync));

            MetricCollector.RegisterMetrics(HttpContext, Request, count);
        }
    }
}