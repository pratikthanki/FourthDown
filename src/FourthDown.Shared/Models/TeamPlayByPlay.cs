using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace FourthDown.Shared.Models
{
    public class TeamPlayByPlay
    {
        public TeamPlayByPlay((string GameId, string PosTeam, int? Down) playKey, List<NflfastrPlayByPlayRow> plays)
        {
            GameId = playKey.GameId;
            Down = playKey.Down;
            PosTeam = playKey.PosTeam;
            TotalPlays = plays.Count;

            RushEpaAverage = 0;
            RushEpaTotal = 0;
            RushEpaSuccess = 0;

            PassEpaAverage = 0;
            PassEpaTotal = 0;
            PassEpaSuccess = 0;

            var rushPlays = plays.Where(x => x.IsRush).ToList();
            if (rushPlays.Count > 0)
            {
                RushEpaAverage = rushPlays.Average(x => x.Epa);
                RushEpaTotal = rushPlays.Sum(x => x.Epa);
                RushEpaSuccess = rushPlays.Count(x => x.Epa > 0) / (double) rushPlays.Count;
            }

            var passPlays = plays.Where(x => x.IsPass).ToList();
            if (passPlays.Count > 0)
            {
                PassEpaAverage = passPlays.Average(x => x.Epa);
                PassEpaTotal = passPlays.Sum(x => x.Epa);
                PassEpaSuccess = passPlays.Count(x => x.Epa > 0) / (double) passPlays.Count;
            }
        }

        public string GameId { get; set; }
        public int? Down { get; set; }
        public string PosTeam { get; set; }
        public int TotalPlays { get; set; }
        public double? RushEpaTotal { get; set; }
        public double? RushEpaAverage { get; set; }
        public double RushEpaSuccess { get; set; }
        public double? PassEpaAverage { get; set; }
        public double? PassEpaTotal { get; set; }
        public double PassEpaSuccess { get; set; }
        
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}