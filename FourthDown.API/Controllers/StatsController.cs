using System.Collections.Generic;
using FourthDown.Api.Models;
using FourthDown.Api.Services;
using Microsoft.AspNetCore.Mvc;
using PlayByPlay = FourthDown.Api.Models.PlayByPlay;

namespace FourthDown.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        private PlayByPlayService _pbpService { get; }

        public StatsController(PlayByPlayService pbpService)
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