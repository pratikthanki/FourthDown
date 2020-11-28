using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using FourthDown.Api.Models;
using FourthDown.Api.Repositories;
using FourthDown.Api.Utilities;
using Jaeger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenTracing;

namespace FourthDown.Api.Controllers
{
    [Route("api/teams")]
    [Authorize]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly ILogger<TeamController> _logger;
        private readonly ITracer _tracer;
        private readonly ITeamRepository _teamRepository;

        public TeamController(
            ILogger<TeamController> logger,
            ITracer tracer,
            ITeamRepository teamRepository)
        {
            _logger = logger;
            _tracer = tracer;
            _teamRepository = teamRepository;
        }

        /// <summary>
        /// Query for all 32 teams 
        /// </summary>
        /// <returns>List of all teams with some details</returns>
        [HttpGet("")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Team>), StatusCodes.Status200OK)]
        public async IAsyncEnumerable<IActionResult> GetTeams(
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            using var scope = _tracer.BuildSpan(nameof(GetTeams)).StartActive();

            _logger.LogInformation("Successful Teams request");

            await foreach (var team in _teamRepository.GetTeamsAsync(cancellationToken))
            {
                yield return Ok(team);
            }
        }
    }
}