using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Shared.Extensions;
using FourthDown.Shared.Models;
using FourthDown.Api.Parameters;
using FourthDown.Api.Schemas;
using FourthDown.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenTracing;

namespace FourthDown.Api.Controllers
{
    [Route("api/schedule")]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ValidationProblemDetailsResponse), StatusCodes.Status400BadRequest)]
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
        /// Query for a range of games and accompanying details.
        /// If a parameter is not given it will default to the current week/season.
        /// When a team is not given, all team games are returned.
        /// </summary>
        /// <param name="queryParameter">Combination of Season, Week and Team</param>
        /// <param name="cancellationToken"></param>
        /// <returns>List of games</returns>
        [HttpGet("")]
        [ProducesResponseType(typeof(ScheduleResponse[]), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Game>>> GetSchedule(
            [FromQuery] ScheduleQueryParameter queryParameter,
            CancellationToken cancellationToken)
        {
            var errors = queryParameter.Validate();
            if (errors.Count > 0)
                return BadRequestError(errors);
            
            var games = await _scheduleService.GetGames(queryParameter, cancellationToken);
            
            _logger.LogInformation("Successful Games request");
            _tracer.ActiveSpan.SetTags(
                HttpContext.Request.GetDisplayUrl(),
                HttpContext.Connection.RemoteIpAddress.MapToIPv6().ToString());

            return Ok(games);
        }

        /// <summary>
        ///     List of results between two teams. Season phase and an offset.
        /// </summary>
        /// <param name="queryParameter">Combination of Season, Week and Team</param>
        /// <param name="cancellationToken"></param>
        /// <returns>List of game scoring summaries</returns>
        [HttpGet("results")]
        [ProducesResponseType(typeof(ScheduleResponse[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetResults(
            [FromQuery] GameResultQueryParameter queryParameter,
            CancellationToken cancellationToken)
        {
            var errors = queryParameter.Validate();
            if (errors.Count > 0)
                return BadRequestError(errors);
            
            var games = await _scheduleService.GetGamesBetween(queryParameter, cancellationToken);
            
            _logger.LogInformation("Successful Games request");
            _tracer.ActiveSpan.SetTags(
                HttpContext.Request.GetDisplayUrl(),
                HttpContext.Connection.RemoteIpAddress.MapToIPv6().ToString());
            
            return Ok(games);
        }

        private BadRequestObjectResult BadRequestError(IDictionary<string, string[]> errors)
        {
            return BadRequest(new ValidationProblemDetails(errors)
            {
                Title = "There are errors with your request.",
                Status = StatusCodes.Status400BadRequest
            });
        }
    }
}