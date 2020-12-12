using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Api.Extensions;
using FourthDown.Api.Models;
using FourthDown.Api.Monitoring;
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
    [ApiVersion("1.0")]
    [Authorize]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status404NotFound)]
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
        ///     If no parameters are passed, all team games for the current week for this season are returned
        /// </summary>
        /// <param name="queryParameter">Combination of Season, Week and Team</param>
        /// <param name="cancellationToken"></param>
        /// <returns>List of games</returns>
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<Game>>> GetSchedule(
            [FromQuery] ScheduleQueryParameter queryParameter,
            CancellationToken cancellationToken)
        {
            using var scope = _tracer.InitializeTrace(HttpContext, nameof(GetSchedule));

            var errors = queryParameter.ValidateBase();
            if (errors.Count > 0)
                return BadRequest(new ValidationProblemDetails(errors)
                {
                    Title = "There are errors with your request.",
                    Status = StatusCodes.Status400BadRequest
                });
            
            scope.LogStart(nameof(_scheduleService.GetGames));

            var games = await _scheduleService.GetGames(queryParameter, cancellationToken);

            scope.LogEnd(nameof(_scheduleService.GetGames));

            _logger.LogInformation("Successful Games request");
            
            _logger.LogInformation(Environment.GetEnvironmentVariable("JAEGER_AGENT_HOST"));
            _logger.LogInformation(Environment.GetEnvironmentVariable("JAEGER_AGENT_PORT"));

            PrometheusMetrics.PathCounter.WithLabels(Request.Method, Request.Path).Inc();
            PrometheusMetrics.RecordsReturned.WithLabels(HttpContext.GetEndpoint().DisplayName).Observe(games.Count());

            return Ok(games);
        }
    }
}