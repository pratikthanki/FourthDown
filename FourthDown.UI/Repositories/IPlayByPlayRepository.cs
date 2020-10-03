using System.Collections.Generic;
using FourthDown.UI.Models;

namespace FourthDown.UI.Repositories
{
    public interface IPlayByPlayRepository
    {
        public IEnumerable<PlayByPlay> GetAllPlays();
        public IEnumerable<PlayByPlay> GetGamePlays(string gameId);
        public PlayByPlay GetGamePlayByPlay(string gameId, string playId);
        public IEnumerable<PlayByPlay> GetTeamPlays(string gameId, string team);
    }
}