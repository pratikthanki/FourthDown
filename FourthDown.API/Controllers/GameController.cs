using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
    [Authorize]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly ILogger<GameController> _logger;
        private readonly ITracer _tracer;
        private IGamePlayService _pbpService;

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
        /// Play by Play data for a set of games
        /// </summary>
        /// <remarks>
        /// Either GameId or a combination of Season, Week and Team should be provided 
        /// </remarks>
        /// <param name="queryParameter">Combination of Season, Week and Team</param>
        /// <param name="cancellationToken"></param>
        /// <returns>List of game play by plays</returns>
        [HttpGet("plays")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<GamePlays>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPlays(
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
            using var scope = _tracer.BuildSpan(nameof(GetPlays)).StartActive();
            
            var plays = await _pbpService.GetGamePlays(queryParameter, cancellationToken);

            if (plays == null || !plays.Any())
                return NotFound(new ValidationProblemDetails(queryParameter.ToKeyValues())
                {
                    Title = "No data for the request parameters given.",
                    Status = StatusCodes.Status404NotFound
                });

            return Ok(plays);
        }

        /// <summary>
        /// game drives summarised from plays 
        /// </summary>
        /// <remarks>
        /// Either GameId or a combination of Season, Week and Team should be provided 
        /// </remarks>
        /// <param name="queryParameter">Combination of Season, Week and Team</param>
        /// <param name="cancellationToken"></param>
        /// <returns>List of game drives</returns>
        [HttpGet("drives")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<GameDrives>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDrives(
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

            using var scope = _tracer.BuildSpan(nameof(GetDrives)).StartActive();

            var plays = await _pbpService.GetGameDrives(queryParameter, cancellationToken);

            if (plays == null || !plays.Any())
                return NotFound(new ValidationProblemDetails(queryParameter.ToKeyValues())
                {
                    Title = "No data for the request parameters given.",
                    Status = StatusCodes.Status404NotFound
                });

            return Ok(plays);
        }

        /// <summary>
        /// List of scoring drives with updated team scores
        /// </summary>
        /// <remarks>
        /// Either GameId or a combination of Season, Week and Team should be provided 
        /// </remarks>
        /// <param name="queryParameter">Combination of Season, Week and Team</param>
        /// <param name="cancellationToken"></param>
        /// <returns>List of game scoring summaries</returns>
        [HttpGet("scoringsummaries")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<GameScoringSummaries>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetScoringSummaries(
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

            using var scope = _tracer.BuildSpan(nameof(GetScoringSummaries)).StartActive();

            var plays = await _pbpService.GetGameScoringSummaries(queryParameter, cancellationToken);

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
