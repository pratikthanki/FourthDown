using System.Collections.Generic;
using FourthDown.Shared.Models;

namespace FourthDown.Collector.Repositories
{
    public interface IPlayByPlayRepository
    {
        public IEnumerable<PlayByPlay> ReadPlays();
    }
}