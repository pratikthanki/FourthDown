using System.Collections.Generic;
using FourthDown.UI.Models;

namespace FourthDown.UI.Services
{
    public interface IPlayByPlayService
    {
        public IEnumerable<PlayByPlay> GetPlayByPlays();
        public IEnumerable<WinProbability> GetGameWinProbability();
    }
}