using System.Collections.Generic;
using FourthDown.Api.Models;

namespace FourthDown.Api.Repositories
{
    public interface IPlayByPlayRepository
    {
        public IEnumerable<PlayByPlay> GetAllPlays();
        public IEnumerable<PlayByPlay> GetGamePlays(string gameId);
        public PlayByPlay GetGamePlayByPlay(string gameId, string playId);
        public IEnumerable<PlayByPlay> GetTeamPlays(string gameId, string team);
    }
}