using System.Collections.Generic;
using System.Threading.Tasks;
using FourthDown.Api.Authentication;
using FourthDown.Api.Extensions;
using FourthDown.Api.Monitoring;
using FourthDown.Api.Parameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenTracing;

namespace FourthDown.Api.Controllers
{
    [Route("api/auth")]
    [ApiVersion("1.0")]
    [Authorize]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthClient _authClient;
        private readonly ITracer _tracer;

        public AuthController(
            IAuthClient authClient,
            ITracer tracer)
        {
            _tracer = tracer;
            _authClient = authClient;
        }

        /// <summary>
        /// Create an API Key with time-based expiry.
        /// </summary>
        /// <param queryParameter="queryParameter">Name/alias for the created API Key</param>
        /// <param name="queryParameter">Name parameter to alias created apiKey</param>
        /// <returns>List of game play by plays</returns>
        [HttpPost]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiKey), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiKey>> CreateApiKey([FromQuery] ApiKeyQueryParameter queryParameter)
        {
            using var scope = _tracer.InitializeTrace(HttpContext, nameof(CreateApiKey));

            PrometheusMetrics.PathCounter.WithLabels(Request.Method, Request.Path).Inc();

            var errors = queryParameter.Validate();
            if (errors.Count > 0)
                return BadRequestErrorValidation(errors);

            var createdKey = await _authClient.CreateApiKey(queryParameter.Name);

            if (createdKey == null)
                return BadRequest(new ValidationProblemDetails()
                {
                    Title = "Unable to create a new apiKey at this moment. Please try again.",
                    Status = StatusCodes.Status400BadRequest
                });

            return Ok(createdKey);
        }

        private BadRequestObjectResult BadRequestErrorValidation(IDictionary<string, string[]> errors)
        {
            return BadRequest(new ValidationProblemDetails(errors)
            {
                Title = "There are errors with your request.",
                Status = StatusCodes.Status400BadRequest
            });
        }
    }
}