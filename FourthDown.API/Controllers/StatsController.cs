using System.Collections.Generic;
using FourthDown.API.Services;
using FourthDown.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using PlayByPlay = FourthDown.Shared.Models.PlayByPlay;

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