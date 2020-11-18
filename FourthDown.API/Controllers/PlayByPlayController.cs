using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Api.Models;
using FourthDown.Api.Parameters;
using FourthDown.Api.Services;
using Microsoft.AspNetCore.Mvc;
using PlayByPlay = FourthDown.Api.Models.PlayByPlay;

namespace FourthDown.Api.Controllers
{
    [Route("api/pbp")]
    [ApiController]
    public class PlayByPlayController : ControllerBase
    {
        private IPlayByPlayService _pbpService { get; }

        public PlayByPlayController(IPlayByPlayService pbpService)
        {
            _pbpService = pbpService;
        }

        [HttpGet("")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PlayByPlay>), 200)]
        public async Task<ActionResult<IEnumerable<PlayByPlay>>> GetPbp(
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

            var plays = await _pbpService.GetPlayByPlays(queryParameter, cancellationToken);

            return Ok(plays);
        }

        [HttpGet("winprobability")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<WinProbability>), 200)]
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
