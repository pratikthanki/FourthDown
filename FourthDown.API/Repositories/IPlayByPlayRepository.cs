using System.Collections.Generic;
using FourthDown.API.Models;

namespace FourthDown.API.Repositories
{
    public interface IPlayByPlayRepository
    {
        public IEnumerable<PlayByPlay> GetAllPlays();
        public IEnumerable<PlayByPlay> GetGamePlays(string gameId);
        public PlayByPlay GetGamePlayByPlay(string gameId, string playId);
        public IEnumerable<PlayByPlay> GetTeamPlays(string gameId, string team);
    }
}