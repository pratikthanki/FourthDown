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

namespace FourthDown.Api.Controllers
{
    [Route("api/game")]
    [Authorize]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly ILogger<ScheduleController> _logger;
        private IPlayByPlayService _pbpService { get; }

        public GameController(
            ILogger<ScheduleController> logger,
            IPlayByPlayService pbpService)
        {
            _logger = logger;
            _pbpService = pbpService;
        }

        /// <summary>
        /// Play by Play and Drive data for a set of games
        /// </summary>
        /// <remarks>
        /// Either GameId or a combination of Season, Week and Team should be provided 
        /// </remarks>
        /// <param name="queryParameter">Combination of Season, Week and Team</param>
        /// <param name="cancellationToken"></param>
        /// <returns>List of game play by plays</returns>
        [HttpGet("playbyplay")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PlayByPlay>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPbp(
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

            var plays = await _pbpService.GetGamePlayByPlays(queryParameter, cancellationToken);

            if (plays == null || !plays.Any())
                return NotFound(new ValidationProblemDetails(queryParameter.ToKeyValues())
                {
                    Title = "No data for the request parameters given.",
                    Status = StatusCodes.Status404NotFound
                });

            return Ok(plays);
        }

        /// <summary>
        /// Win probabilities represented per drive
        /// </summary>
        /// <remarks>
        /// Either GameId or a combination of Season, Week and Team should be provided 
        /// </remarks>
        /// <param name="queryParameter">Combination of Season, Week and Team</param>
        /// <param name="cancellationToken"></param>
        /// <returns>List of game win probabilities</returns>
        [HttpGet("winprobability")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<WinProbability>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<WinProbability>>> GetWp(
            [FromQuery] PlayByPlayQueryParameter queryParameter,
            CancellationToken cancellationToken)
        {
            var errors = queryParameter.Validate();
            if (errors.Count > 0)
                return BadRequest(new ValidationProblemDetails(errors)
                {
                    Title = "Looks like there are some errors with your request.",
                    Status = 400,
                });

            var wp = await _pbpService.GetGameWinProbability(queryParameter, cancellationToken);
            
            if (wp == null || !wp.Any())
                return NotFound();
            
            return Ok(wp);
        }
    }
}
