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
    [Route("api/teams")]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [ResponseCache(Duration = 86400, VaryByQueryKeys = new[] {"*"})]
    [ProducesResponseType(typeof(TeamResponse[]), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetailsResponse),StatusCodes.Status400BadRequest)]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ITracer _tracer;

        public TeamController(
            ITracer tracer,
            ITeamRepository teamRepository, 
            IWebHostEnvironment webHostEnvironment)
        {
            _tracer = tracer;
            _teamRepository = teamRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        ///     Query for all 32 teams
        /// </summary>
        /// <returns>List of all teams with details</returns>
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<Team>>> GetTeams(CancellationToken cancellationToken)
        {
            var webRootPath = _webHostEnvironment.WebRootPath;

            var teams = await _teamRepository.GetTeamsAsync(webRootPath, cancellationToken);
            
            return Ok(teams);
        }
    }
}