using System.Collections.Generic;
using FourthDown.UI.Models;
using FourthDown.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace FourthDown.UI.Controllers
{
    [Route("stats/")]
    public class StatsController : ControllerBase
    {
        public StatsController(JsonPlayByPlayService playByPlayService)
        {
            PlayByPlayService = playByPlayService;
        }

        private JsonPlayByPlayService PlayByPlayService { get; }

        [HttpGet]
        [Route("playbyplay")]
        public IEnumerable<PlayByPlay> GetPbp()
        {
            return PlayByPlayService.GetPlayByPlays();
        }
        [HttpGet]
        [Route("winprobability")]
        public IEnumerable<WinProbability> GetWp()
        {
            return PlayByPlayService.GetGameWinProbability();
        }
    }
}