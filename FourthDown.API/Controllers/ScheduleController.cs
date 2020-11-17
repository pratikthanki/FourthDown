using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Api.Models;
using FourthDown.Api.Parameters;
using FourthDown.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace FourthDown.Api.Controllers
{
    [Route("api/schedule")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;

        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        /// <summary>
        /// Query for games based on conditions
        /// </summary>
        /// <remarks>
        /// If no parameters are passed, games for the current week/season and all teams is returned
        /// </remarks>
        /// <param name="queryParameter">Combination of Season, Week and Team</param>
        /// <param name="cancellationToken"></param>
        /// <returns>List of Games</returns>
        [HttpGet("")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Game>), 200)]
        public async Task<ActionResult<IEnumerable<Game>>> GetSchedule(
            [FromQuery] ScheduleQueryParameter queryParameter,
            CancellationToken cancellationToken)
        {
            var games = await _scheduleService.GetGames(queryParameter, cancellationToken);

            return Ok(games);
        }
    }
}