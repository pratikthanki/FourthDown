using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Api.Extensions;
using FourthDown.Api.Models;
using FourthDown.Api.Monitoring;
using FourthDown.Api.Parameters;
using FourthDown.Api.Schemas;
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
    [ProducesResponseType(typeof(ScheduleResponse[]), StatusCodes.Status200OK)]
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
        public async Task<ActionResult<IEnumerable<Game>>> GetSchedule(
            [FromQuery] ScheduleQueryParameter queryParameter,
            CancellationToken cancellationToken)
        {
            using var scope = _tracer.InitializeTrace(HttpContext, nameof(GetSchedule));

            var errors = queryParameter.Validate();
            if (errors.Count > 0)
                return BadRequestError(errors);

            scope.LogStart(nameof(_scheduleService.GetGames));

            var games = await _scheduleService.GetGames(queryParameter, cancellationToken);

            scope.LogEnd(nameof(_scheduleService.GetGames));

            _logger.LogInformation("Successful Games request");

            MetricCollector.RegisterMetrics(HttpContext, Request);
            PrometheusMetrics.RecordsReturned
                .WithLabels(Request.Method, HttpContext.GetEndpoint().DisplayName)
                .Observe(games.ToList().Count);

            return Ok(games);
        }

        /// <summary>
        ///     List of results between two teams. Season phase and an offset.
        /// </summary>
        /// <param name="queryParameter">Combination of Season, Week and Team</param>
        /// <param name="cancellationToken"></param>
        /// <returns>List of game scoring summaries</returns>
        [HttpGet("results")]
        [ProducesResponseType(typeof(GameScoringSummaries[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetResults(
            [FromQuery] GameResultQueryParameter queryParameter,
            CancellationToken cancellationToken)
        {
            using var scope = _tracer.InitializeTrace(HttpContext, nameof(GetResults));
            MetricCollector.RegisterMetrics(HttpContext, Request);

            var errors = queryParameter.Validate();
            if (errors.Count > 0)
                return BadRequestError(errors);

            scope.LogStart(nameof(_scheduleService.GetGames));

            var games = await _scheduleService.GetGamesBetween(queryParameter, cancellationToken);

            scope.LogEnd(nameof(_scheduleService.GetGames));

            _logger.LogInformation("Successful Games request");

            MetricCollector.RegisterMetrics(HttpContext, Request);
            PrometheusMetrics.RecordsReturned
                .WithLabels(Request.Method, HttpContext.GetEndpoint().DisplayName)
                .Observe(games.ToList().Count);

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