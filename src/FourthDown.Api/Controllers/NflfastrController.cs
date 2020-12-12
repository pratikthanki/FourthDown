using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Api.Extensions;
using FourthDown.Api.Models;
using FourthDown.Api.Monitoring;
using FourthDown.Api.Parameters;
using FourthDown.Api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenTracing;

namespace FourthDown.Api.Controllers
{
    [Route("api/nflfastr")]
    [ApiVersion("1.0")]
    [Authorize]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status404NotFound)]
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
        ///     Play by Play data for a set of games
        ///     Either GameId or a combination of Season, Week and Team should be provided
        /// </summary>
        /// <param name="queryParameter">Combination of Season, Week and Team</param>
        /// <param name="cancellationToken"></param>
        /// <returns>List of game play by plays</returns>
        [HttpGet("")]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<PlayByPlay>>> GetPlayByPlays(
            [FromQuery] PlayByPlayQueryParameter queryParameter,
            CancellationToken cancellationToken)
        {
            using var scope = _tracer.InitializeTrace(HttpContext, nameof(GetPlayByPlays));

            scope.LogStart(nameof(_playByPlayRepository.GetPlayByPlaysAsync));

            var plays = await _playByPlayRepository.GetPlayByPlaysAsync(queryParameter, cancellationToken);

            scope.LogEnd(nameof(_playByPlayRepository.GetPlayByPlaysAsync));

            if (plays == null || !plays.Any())
                return NotFound(new ValidationProblemDetails
                {
                    Title = $"No play by play data for the season {queryParameter.Season} found.",
                    Status = StatusCodes.Status404NotFound
                });

            MetricCollector.RegisterMetrics(HttpContext, Request, plays.Count());

            return Ok(plays);
        }
    }
}