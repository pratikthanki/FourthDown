using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Api.Extensions;
using FourthDown.Api.Models;
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
        private readonly ILogger<GameController> _logger;
        private readonly ITracer _tracer;
        private readonly IGamePlayService _pbpService;

        public GameController(
            ILogger<GameController> logger,
            ITracer tracer,
            IGamePlayService pbpService)
        {
            _logger = logger;
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
        public async Task<ActionResult<IEnumerable<GamePlays>>> GetPlays(
            [FromQuery] PlayByPlayQueryParameter queryParameter,
            CancellationToken cancellationToken)
        {
            var errors = queryParameter.Validate();
            if (errors.Count > 0)
                return BadRequest(new ValidationProblemDetails(errors)
                {
                    Title = "Looks like there are some errors with your request.",
                    Status = StatusCodes.Status400BadRequest
                });

            using var scope = _tracer.InitializeTrace(HttpContext, nameof(GetPlays));

            scope.LogStart(nameof(_pbpService.GetGamePlaysAsync));

            var plays = await _pbpService.GetGamePlaysAsync(queryParameter, cancellationToken);

            scope.LogEnd(nameof(_pbpService.GetGamePlaysAsync));

            if (plays == null || !plays.Any())
                return NotFound(new ValidationProblemDetails(queryParameter.ToKeyValues())
                {
                    Title = "No data for the request parameters given.",
                    Status = StatusCodes.Status404NotFound
                });

            return Ok(plays);
        }

        /// <summary>
        ///     Game drives summarised from plays
        ///     Either GameId or a combination of Season, Week and Team should be provided
        /// </summary>
        /// <param name="queryParameter">Combination of Season, Week and Team</param>
        /// <param name="cancellationToken"></param>
        /// <returns>List of game drives</returns>
        [HttpGet("drives")]
        public async Task<ActionResult<IEnumerable<GameDrives>>> GetDrives(
            [FromQuery] PlayByPlayQueryParameter queryParameter,
            CancellationToken cancellationToken)
        {
            var errors = queryParameter.Validate();
            if (errors.Count > 0)
                return BadRequest(new ValidationProblemDetails(errors)
                {
                    Title = "Looks like there are some errors with your request.",
                    Status = StatusCodes.Status400BadRequest
                });

            using var scope = _tracer.InitializeTrace(HttpContext, nameof(GetPlays));

            scope.LogStart(nameof(_pbpService.GetGameDrives));

            var plays = await _pbpService.GetGameDrives(queryParameter, cancellationToken);

            scope.LogEnd(nameof(_pbpService.GetGameDrives));

            if (plays == null || !plays.Any())
                return NotFound(new ValidationProblemDetails(queryParameter.ToKeyValues())
                {
                    Title = "No data for the request parameters given.",
                    Status = StatusCodes.Status404NotFound
                });

            return Ok(plays);
        }

        /// <summary>
        ///     List of scoring drives with updated team scores
        ///     Either GameId or a combination of Season, Week and Team should be provided
        /// </summary>
        /// <param name="queryParameter">Combination of Season, Week and Team</param>
        /// <param name="cancellationToken"></param>
        /// <returns>List of game scoring summaries</returns>
        [HttpGet("scoringsummaries")]
        public async Task<ActionResult<IEnumerable<GameScoringSummaries>>> GetScoringSummaries(
            [FromQuery] PlayByPlayQueryParameter queryParameter,
            CancellationToken cancellationToken)
        {
            var errors = queryParameter.Validate();
            if (errors.Count > 0)
                return BadRequest(new ValidationProblemDetails(errors)
                {
                    Title = "Looks like there are some errors with your request.",
                    Status = StatusCodes.Status400BadRequest
                });

            using var scope = _tracer.InitializeTrace(HttpContext, nameof(GetPlays));

            scope.LogStart(nameof(_pbpService.GetGameScoringSummaries));

            var plays = await _pbpService.GetGameScoringSummaries(queryParameter, cancellationToken);

            scope.LogEnd(nameof(_pbpService.GetGameScoringSummaries));

            if (plays == null || !plays.Any())
                return NotFound(new ValidationProblemDetails(queryParameter.ToKeyValues())
                {
                    Title = "No data for the request parameters given.",
                    Status = StatusCodes.Status404NotFound
                });

            return Ok(plays);
        }
    }
}