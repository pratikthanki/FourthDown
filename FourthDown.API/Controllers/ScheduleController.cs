using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Api.Models;
using FourthDown.Api.Repositories;
using FourthDown.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace FourthDown.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IGameRepository _gameRepository;
        private readonly IScheduleService _scheduleService;

        public ScheduleController(
            IGameRepository gameRepository,
            IScheduleService scheduleService)
        {
            _gameRepository = gameRepository;
            _scheduleService = scheduleService;
        }

        [HttpGet("")]
        public async Task<IEnumerable<Game>> GetSchedule(CancellationToken cancellationToken)
        {
            return await _scheduleService.GetGamesForCurrentWeek(cancellationToken);
        }
        
        [HttpGet("{week}")]
        public async Task<IEnumerable<Game>> GetScheduleForWeek(int week, CancellationToken cancellationToken)
        {
            return await _scheduleService.GetGamesForWeek(week, cancellationToken);
        }
    }
}