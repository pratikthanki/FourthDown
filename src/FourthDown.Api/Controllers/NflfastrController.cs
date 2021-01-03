using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Api.Extensions;
using FourthDown.Api.Models;
using FourthDown.Api.Parameters;
using FourthDown.Api.Repositories;
using FourthDown.Api.Schemas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenTracing;

namespace FourthDown.Api.Controllers
{
    [Route("api/nflfastr")]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetailsResponse), StatusCodes.Status400BadRequest)]
    [ResponseCache(Duration = 600, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new[] {"impactlevel", "pii"})]
    [ApiController]
    public class NflfastrController : ControllerBase
    {
        private readonly ILogger<NflfastrController> _logger;
        private readonly ITracer _tracer;
        private readonly IPlayByPlayRepository _playByPlayRepository;

        public NflfastrController(
            IPlayByPlayRepository playByPlayRepository,
            ILogger<NflfastrController> logger,
            ITracer tracer)
        {
            _playByPlayRepository = playByPlayRepository;
            _logger = logger;
            _tracer = tracer;
        }

        /// <summary>
        /// Play-by-Play data for a set of games.
        /// Either GameId or a combination of Season, Week and Team should be provided.
        /// Game data sourced from the R package nflfastrR.
        /// </summary>
        /// <param name="queryParameter">Combination of Season, Week and Team</param>
        /// <param name="cancellationToken"></param>
        /// <returns>List of game play by plays</returns>
        [HttpGet("")]
        [ProducesResponseType(typeof(PlayByPlayResponse[]), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PlayByPlay>>> GetPlayByPlays(
            [FromQuery] NflfastrQueryParameter queryParameter,
            CancellationToken cancellationToken)
        {
            var errors = queryParameter.Validate();
            if (errors.Count > 0)
                return BadRequest(new ValidationProblemDetails(errors)
                {
                    Title = "There are errors with your request.",
                    Status = StatusCodes.Status400BadRequest
                });

            var queryOptions = new PlayByPlayQueryParameter() {Season = queryParameter.Season};
            var plays = await _playByPlayRepository.GetPlayByPlaysAsync(queryOptions, cancellationToken);

            _tracer.ActiveSpan.SetTags(HttpContext);

            return Ok(plays);
        }
    }
}