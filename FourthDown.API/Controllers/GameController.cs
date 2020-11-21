using System.Collections.Generic;
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
        [ProducesResponseType(typeof(IEnumerable<GameDetail>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GameDetail>>> GetPbp(
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

            var plays = await _pbpService.GetGamePlayByPlays(queryParameter, cancellationToken);

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

            return Ok(wp);
        }
    }
}
