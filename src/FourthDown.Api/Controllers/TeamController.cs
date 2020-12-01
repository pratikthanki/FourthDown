using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using FourthDown.Api.Extensions;
using FourthDown.Api.Models;
using FourthDown.Api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenTracing;

namespace FourthDown.Api.Controllers
{
    [Route("api/teams")]
    [ApiVersion("1.0")]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async IAsyncEnumerable<Team> GetTeams([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            using var scope = _tracer.InitializeTrace(HttpContext, nameof(GetTeams));

            scope.LogStart(nameof(_teamRepository.GetTeamsAsync));

            var teams = await _teamRepository.GetTeamsAsync(cancellationToken);

            scope.LogEnd(nameof(_teamRepository.GetTeamsAsync));

            foreach (var team in teams)
            {
                yield return team;
            }
        }
    }
}