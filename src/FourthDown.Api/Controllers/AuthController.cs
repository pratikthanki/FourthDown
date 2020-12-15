using System.Threading;
using System.Threading.Tasks;
using FourthDown.Api.Monitoring;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenTracing;

namespace FourthDown.Api.Controllers
{
    [Route("api/auth")]
    [ApiVersion( "1.0" )]
    [Authorize]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly ITracer _tracer;

        public AuthController(
            ILogger<AuthController> logger,
            ITracer tracer)
        {
            _logger = logger;
            _tracer = tracer;
        }

        /// <summary>
        /// Create an API Key with time-based expiry.
        /// </summary>
        /// <param name="name">Name/alias for the created API Key</param>
        /// <param name="cancellationToken"></param>
        /// <returns>List of game play by plays</returns>
        [HttpPost]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> CreateApiKey(
            [FromQuery] string name,
            CancellationToken cancellationToken)
        {
            using var scope = _tracer.BuildSpan(nameof(CreateApiKey)).StartActive();
            
            PrometheusMetrics.PathCounter.WithLabels(Request.Method, Request.Path).Inc();

            return Ok();
        }
    }
}