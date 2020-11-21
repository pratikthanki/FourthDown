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
    [Route("api/schedule")]
    [Authorize]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly ILogger<ScheduleController> _logger;
        private readonly IScheduleService _scheduleService;

        public ScheduleController(
            ILogger<ScheduleController> logger,
            IScheduleService scheduleService)
        {
            _logger = logger;
            _scheduleService = scheduleService;
        }

        /// <summary>
        /// Query for a set of games and accompanying details
        /// </summary>
        /// <remarks>
        /// If no parameters are passed, all team games for the current week for this season are returned
        /// </remarks>
        /// <param name="queryParameter">Combination of Season, Week and Team</param>
        /// <param name="cancellationToken"></param>
        /// <returns>List of games</returns>
        [HttpGet("")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Game>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Game>>> GetSchedule(
            [FromQuery] ScheduleQueryParameter queryParameter,
            CancellationToken cancellationToken)
        {
            var games = await _scheduleService.GetGames(queryParameter, cancellationToken);

            _logger.LogInformation("Successful Games request");

            return Ok(games);
        }
    }
}