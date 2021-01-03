using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Api.Extensions;
using FourthDown.Api.Models;
using FourthDown.Api.Parameters;
using FourthDown.Api.Schemas;
using FourthDown.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenTracing;
#pragma warning disable 1998

namespace FourthDown.Api.Controllers
{
    [Route("api/game")]
    [ApiVersion("1.0")]
    [ApiController]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ValidationProblemDetailsResponse), StatusCodes.Status400BadRequest)]
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
        ///     Play by Play data for a set of games. 
        ///     Either GameId or a combination of Season, Week and Team should be provided.
        /// </summary>
        /// <param name="queryParameter">Combination of Season, Week and Team</param>
        /// <param name="cancellationToken"></param>
        /// <returns>List of game play by plays</returns>
        [HttpGet("plays")]
        [ProducesResponseType(typeof(GamePlays[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPlays(
            [FromQuery] PlayByPlayQueryParameter queryParameter,
            CancellationToken cancellationToken)
        {
            var errors = queryParameter.Validate();
            if (errors.Count > 0)
                return BadRequestErrorValidation(errors);

            var plays = _pbpService.GetGamePlaysAsync(queryParameter, cancellationToken);

            _tracer.ActiveSpan.SetTags(HttpContext);

            return Ok(plays);
        }

        /// <summary>
        ///     Game drives summarised from plays. 
        ///     Either GameId or a combination of Season, Week and Team should be provided.
        /// </summary>
        /// <param name="queryParameter">Combination of Season, Week and Team</param>
        /// <param name="cancellationToken"></param>
        /// <returns>List of game drives</returns>
        [HttpGet("drives")]
        [ProducesResponseType(typeof(GameDrives[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDrives(
            [FromQuery] PlayByPlayQueryParameter queryParameter,
            CancellationToken cancellationToken)
        {
            var errors = queryParameter.Validate();
            if (errors.Count > 0)
                return BadRequestErrorValidation(errors);

            var plays = _pbpService.GetGameDrivesAsync(queryParameter, cancellationToken);

            _tracer.ActiveSpan.SetTags(HttpContext);

            return Ok(plays);
        }

        /// <summary>
        ///     List of scoring drives with updated team scores. 
        ///     Either GameId or a combination of Season, Week and Team should be provided.
        /// </summary>
        /// <param name="queryParameter">Combination of Season, Week and Team</param>
        /// <param name="cancellationToken"></param>
        /// <returns>List of game scoring summaries</returns>
        [HttpGet("scoringsummaries")]
        [ProducesResponseType(typeof(GameScoringSummaries[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetScoringSummaries(
            [FromQuery] PlayByPlayQueryParameter queryParameter,
            CancellationToken cancellationToken)
        {
            var errors = queryParameter.Validate();
            if (errors.Count > 0)
                return BadRequestErrorValidation(errors);

            var plays = _pbpService.GetGameScoringSummariesAsync(queryParameter, cancellationToken);

            _tracer.ActiveSpan.SetTags(HttpContext);

            return Ok(plays);
        }

        private BadRequestObjectResult BadRequestErrorValidation(IDictionary<string, string[]> errors)
        {
            return BadRequest(new ValidationProblemDetails(errors)
            {
                Title = "There are errors with your request.",
                Status = StatusCodes.Status400BadRequest
            });
        }
    }
}