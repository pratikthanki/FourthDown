using System.Collections.Generic;
using FourthDown.API.Models;

namespace FourthDown.API.Services
{
    public interface IPlayByPlayService
    {
        public IEnumerable<PlayByPlay> GetPlayByPlays();
        public IEnumerable<WinProbability> GetGameWinProbability();
    }
}