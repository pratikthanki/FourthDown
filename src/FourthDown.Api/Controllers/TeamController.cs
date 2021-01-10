using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Api.Models;
using FourthDown.Api.Repositories;
using FourthDown.Api.Schemas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenTracing;

namespace FourthDown.Api.Controllers
{
    [Route("api/teams")]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(TeamResponse[]), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetailsResponse),StatusCodes.Status400BadRequest)]
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
            var teams = await _teamRepository.GetTeamsAsync(cancellationToken);
            
            return Ok(teams);
        }
    }
}