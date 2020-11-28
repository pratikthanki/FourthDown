using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Api.Extensions;
using FourthDown.Api.Models;
using FourthDown.Api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenTracing;
using OpenTracing.Tag;

namespace FourthDown.Api.Controllers
{
    [Route("api/teams")]
    [ApiVersion( "1.0" )]
    [Authorize]
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
        /// <returns>List of all teams with some details</returns>
        [HttpGet("")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Team>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTeams(CancellationToken cancellationToken)
        {
            using var scope = _tracer.InitializeTrace(nameof(GetTeams));
            
            scope.LogStart(nameof(_teamRepository.GetTeamsAsync));

            var team = await _teamRepository.GetTeamsAsync(cancellationToken);

            scope.LogEnd(nameof(_teamRepository.GetTeamsAsync));
            
            return Ok(team);
        }
    }
}