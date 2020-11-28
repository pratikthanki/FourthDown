using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Api.Models;
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
    [Authorize]
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
        /// </summary>
        /// <remarks>
        ///     Either GameId or a combination of Season, Week and Team should be provided
        /// </remarks>
        /// <param name="queryParameter">Combination of Season, Week and Team</param>
        /// <param name="cancellationToken"></param>
        /// <returns>List of game play by plays</returns>
        [HttpGet("")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PlayByPlay),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPlayByPlays(
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

            using var scope = _tracer.BuildSpan(nameof(GetPlayByPlays)).StartActive();

            var plays = await _playByPlayRepository.GetPlayByPlaysAsync(queryParameter, cancellationToken);

            if (plays == null || !plays.Any())
                return NotFound(new ValidationProblemDetails
                {
                    Title = $"No play by play data for the season {queryParameter.Season} found.",
                    Status = StatusCodes.Status404NotFound
                });

            return Ok(plays);
        }
    }
}