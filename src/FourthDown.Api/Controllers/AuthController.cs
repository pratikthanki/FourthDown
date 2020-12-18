using System.Threading.Tasks;
using FourthDown.Api.Authentication;
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
        private readonly IAuthClient _authClient;
        private readonly ILogger<AuthController> _logger;
        private readonly ITracer _tracer;

        public AuthController(
            IAuthClient authClient,
            ILogger<AuthController> logger,
            ITracer tracer)
        {
            _logger = logger;
            _tracer = tracer;
            _authClient = authClient;
        }

        /// <summary>
        /// Create an API Key with time-based expiry.
        /// </summary>
        /// <param name="name">Name/alias for the created API Key</param>
        /// <returns>List of game play by plays</returns>
        [HttpPost]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiKey), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiKey>> CreateApiKey([FromQuery] string name)
        {
            using var scope = _tracer.BuildSpan(nameof(CreateApiKey)).StartActive();
            
            PrometheusMetrics.PathCounter.WithLabels(Request.Method, Request.Path).Inc();

            var createdKey = await _authClient.CreateApiKey(name);

            if (createdKey == null)
                return BadRequest(new ValidationProblemDetails()
                {
                    Title = "Unable to create a new apiKey at this moment. Please try again.",
                    Status = StatusCodes.Status400BadRequest
                });

            return Ok(createdKey);
        }
    }
}