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
using OpenTracing;

namespace FourthDown.Api.Controllers
{
    [Route("api/schedule")]
    [Authorize]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly ILogger<ScheduleController> _logger;
        private readonly IScheduleService _scheduleService;
        private readonly ITracer _tracer;

        public ScheduleController(
            ILogger<ScheduleController> logger,
            IScheduleService scheduleService,
            ITracer tracer)
        {
            _logger = logger;
            _scheduleService = scheduleService;
            _tracer = tracer;
        }

        /// <summary>
        ///     Query for a set of games and accompanying details
        /// </summary>
        /// <remarks>
        ///     If no parameters are passed, all team games for the current week for this season are returned
        /// </remarks>
        /// <param name="queryParameter">Combination of Season, Week and Team</param>
        /// <param name="cancellationToken"></param>
        /// <returns>List of games</returns>
        [HttpGet("")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Game>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetSchedule(
            [FromQuery] ScheduleQueryParameter queryParameter,
            CancellationToken cancellationToken)
        {
            using var scope = _tracer.BuildSpan(nameof(GetSchedule)).StartActive();

            var games = await _scheduleService.GetGames(queryParameter, cancellationToken);

            _logger.LogInformation("Successful Games request");

            return Ok(games);
        }
    }
}