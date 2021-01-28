using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Shared.Extensions;
using FourthDown.Shared.Models;
using FourthDown.Api.Parameters;
using FourthDown.Api.Schemas;
using FourthDown.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenTracing;

namespace FourthDown.Api.Controllers
{
    [Route("api/nflfastr")]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetailsResponse), StatusCodes.Status400BadRequest)]
    [ResponseCache(Duration = 600, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new[] {"impactlevel", "pii"})]
    [ApiController]
    public class NflfastrController : ControllerBase
    {
        private readonly ILogger<NflfastrController> _logger;
        private readonly ITracer _tracer;
        private readonly INflfastrService _nflfastrService;

        public NflfastrController(
            INflfastrService nflfastrService,
            ILogger<NflfastrController> logger,
            ITracer tracer)
        {
            _nflfastrService = nflfastrService;
            _logger = logger;
            _tracer = tracer;
        }

        /// <summary>
        /// Play-by-Play data for a set of games.
        /// Either GameId or a combination of Season, Week and Team should be provided.
        /// Game data sourced from the R package nflfastrR.
        /// </summary>
        /// <param name="queryParameter">Combination of Season, Week and Team</param>
        /// <param name="cancellationToken"></param>
        /// <returns>List of game play by plays</returns>
        [HttpGet("")]
        [ProducesResponseType(typeof(TeamPlayByPlay[]), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TeamPlayByPlay>>> GetPlayByPlays(
            [FromQuery] NflfastrQueryParameter queryParameter,
            CancellationToken cancellationToken)
        {
            var errors = queryParameter.Validate();
            if (errors.Count > 0)
                return BadRequest(new ValidationProblemDetails(errors)
                {
                    Title = "There are errors with your request.",
                    Status = StatusCodes.Status400BadRequest
                });

            var plays = await _nflfastrService.GetSummarisedStats(queryParameter, cancellationToken);

            _tracer.ActiveSpan.SetTags(
                HttpContext.Request.GetDisplayUrl(),
                HttpContext.Connection.RemoteIpAddress.MapToIPv6().ToString());

            return Ok(plays);
        }
    }
}