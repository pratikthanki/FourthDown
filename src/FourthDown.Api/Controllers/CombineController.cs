using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Shared.Models;
using FourthDown.Shared.Repositories;
using FourthDown.Api.Schemas;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenTracing;

namespace FourthDown.Api.Controllers
{
    [Route("api/combine")]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(CombineWorkoutResponse[]), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetailsResponse), StatusCodes.Status400BadRequest)]
    [ApiController]
    public class CombineController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICombineRepository _combineRepository;
        private readonly ITracer _tracer;

        public CombineController(
            ITracer tracer,
            IWebHostEnvironment webHostEnvironment,
            ICombineRepository combineRepository)
        {
            _tracer = tracer;
            _combineRepository = combineRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        ///     Get combine workouts for all players
        /// </summary>
        /// <returns>Players with no workout data will show `null`</returns>
        /// <param name="season"></param>
        /// <param name="cancellationToken"></param>
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<CombineWorkout>>> Get(
            [FromQuery] int season,
            CancellationToken cancellationToken)
        {
            var errors = Validate(season);
            if (errors.Count > 1)
                return BadRequest(new ValidationProblemDetails(errors)
                {
                    Title = "There are errors with your request.",
                    Status = StatusCodes.Status400BadRequest
                });

            var webRootPath = _webHostEnvironment.WebRootPath;

            var teams = await _combineRepository.GetCombineSummaryAsync(
                webRootPath,
                season,
                cancellationToken);

            return Ok(teams);
        }

        private static Dictionary<string, string[]> Validate(int season)
        {
            var Today = DateTime.UtcNow;
            var currentSeason = Today.Month > 8 ? Today.Year : Today.Year - 1;

            var errors = new Dictionary<string, string[]>();

            if (season < 2012 || season > currentSeason)
                errors["season"] = new[] {$"Season should be between 2012 and {currentSeason} inclusive"};

            return errors;
        }
    }
}