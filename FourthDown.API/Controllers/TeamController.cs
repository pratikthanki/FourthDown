using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Api.Models;
using FourthDown.Api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FourthDown.Api.Controllers
{
    [Route("api/teams")]
    [Authorize]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly ILogger<TeamController> _logger;
        private ITeamRepository _teamRepository { get; }

        public TeamController(
            ILogger<TeamController> logger,
            ITeamRepository teamRepository)
        {
            _logger = logger;
            _teamRepository = teamRepository;
        }

        /// <summary>
        /// Query for all 32 teams 
        /// </summary>
        /// <returns>List of all teams with some details</returns>
        [HttpGet("")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Team>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Team>>> GetTeams(CancellationToken cancellationToken)
        {
            var teams = await _teamRepository.GetTeams(cancellationToken);
            
            _logger.LogInformation("Successful Teams request");

            return Ok(teams);
        }
    }
}