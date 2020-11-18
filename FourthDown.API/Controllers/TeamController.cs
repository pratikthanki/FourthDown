using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Api.Models;
using FourthDown.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FourthDown.Api.Controllers
{
    [Route("api/team")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private ITeamRepository _teamRepository { get; }

        public TeamController(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        /// <summary>
        /// Query for all 32 teams in the league
        /// </summary>
        [HttpGet("")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Team>), 200)]
        public async Task<ActionResult<IEnumerable<Team>>> GetTeams(CancellationToken cancellationToken)
        {
            var teams = await _teamRepository.GetTeams(cancellationToken);

            return Ok(teams);
        }
    }
}