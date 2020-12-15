using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Api.Extensions;
using FourthDown.Api.Models;
using FourthDown.Api.Monitoring;
using FourthDown.Api.Repositories;
using FourthDown.Api.Schemas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenTracing;

namespace FourthDown.Api.Controllers
{
    [Route("api/teams")]
    [ApiVersion("1.0")]
    [Authorize]
    [Produces("application/json")]
    [ProducesResponseType(typeof(TeamResponse[]), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetailsResponse),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ValidationProblemDetailsResponse), StatusCodes.Status404NotFound)]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly ITeamRepository _teamRepository;
        private readonly ITracer _tracer;

        public TeamController(
            ITracer tracer,
            ITeamRepository teamRepository)
        {
            _tracer = tracer;
            _teamRepository = teamRepository;
        }

        /// <summary>
        ///     Query for all 32 teams
        /// </summary>
        /// <returns>List of all teams with details</returns>
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<Team>>> GetTeams(CancellationToken cancellationToken)
        {
            using var scope = _tracer.InitializeTrace(HttpContext, nameof(GetTeams));

            scope.LogStart(nameof(_teamRepository.GetTeamsAsync));

            var teams = await _teamRepository.GetTeamsAsync(cancellationToken);

            scope.LogEnd(nameof(_teamRepository.GetTeamsAsync));

            MetricCollector.RegisterMetrics(HttpContext, Request, teams.Count());

            return Ok(teams);
        }
    }
}