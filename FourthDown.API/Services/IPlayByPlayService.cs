using System.Collections.Generic;
using FourthDown.Shared.Models;
using PlayByPlay = FourthDown.Shared.Models.PlayByPlay;

namespace FourthDown.API.Services
{
    public interface IPlayByPlayService
    {
        public IEnumerable<PlayByPlay> GetPlayByPlays();
        public IEnumerable<PlayByPlay> GetGamePlayByPlays(int gameId);
        public IEnumerable<WinProbability> GetGameWinProbability();
    }
}