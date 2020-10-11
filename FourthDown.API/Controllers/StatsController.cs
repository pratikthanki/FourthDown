using System.Collections.Generic;
using System.Threading.Tasks;
using FourthDown.API.Models;
using FourthDown.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace FourthDown.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        private JsonPlayByPlayService _pbpService { get; }

        public StatsController(JsonPlayByPlayService pbpService)
        {
            _pbpService = pbpService;
        }

        [HttpGet]
        [Route("playbyplay")]
        public IEnumerable<PlayByPlay> GetPbp()
        {
            return _pbpService.GetPlayByPlays();
        }

        [HttpGet]
        [Route("winprobability")]
        public IEnumerable<WinProbability> GetWp()
        {
            return _pbpService.GetGameWinProbability();
        }
    }
}