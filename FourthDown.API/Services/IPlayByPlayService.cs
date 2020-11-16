using System.Collections.Generic;
using FourthDown.Api.Models;
using PlayByPlay = FourthDown.Api.Models.PlayByPlay;

namespace FourthDown.Api.Services
{
    public interface IPlayByPlayService
    {
        public IEnumerable<PlayByPlay> GetPlayByPlays();
        public IEnumerable<PlayByPlay> GetGamePlayByPlays(int gameId);
        public IEnumerable<WinProbability> GetGameWinProbability();
    }
}