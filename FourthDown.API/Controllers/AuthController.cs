using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FourthDown.Api.Controllers
{
    [Route("api/auth")]
    [Authorize]
    [ApiController]
    public class AuthController : ControllerBase
    {
        /// <summary>
        /// Create an API Key
        /// </summary>
        /// <remarks>
        /// Create an API Key with an expiry 
        /// </remarks>
        /// <param name="name">Name/alias for the created API Key</param>
        /// <param name="cancellationToken"></param>
        /// <returns>List of game play by plays</returns>
        [HttpPost]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        public async Task<ActionResult> CreateApiKey(
            [FromQuery] string name, 
            CancellationToken cancellationToken)
        {
            return Ok();
        }
    }
}