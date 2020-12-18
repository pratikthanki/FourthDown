using System;
using System.Collections.Generic;
using System.Linq;
using FourthDown.Api.Utilities;

namespace FourthDown.Api.Parameters
{
    /// <summary>
    /// </summary>
    public class PlayByPlayQueryParameter
    {
        /// <summary>
        /// Week based on the football schedule:
        /// ```
        /// - REG (1-17)
        /// - Wild Card (18)
        /// - Divisional (19)
        /// - Conference (20)
        /// - Super Bowl (21)
        /// ```
        /// </summary>
        /// <remarks>
        /// Defaults to the current week if not set.
        /// </remarks>
        public int? Week { get; set; }

        /// <summary>
        /// Valid seasons from 1999 to the current season.
        /// </summary>
        /// <remarks>
        /// Defaults to the current season if not set.
        /// </remarks>
        public int? Season { get; set; }

        /// <summary>
        /// Team abbreviation. See `/api/teams` for the `abr` attribute.
        /// </summary>
        /// <remarks>
        /// Returns data for all teams if not set.
        /// </remarks>
        public string Team { get; set; }

        /// <summary>
        /// Single or comma-separated list of GameId's.
        /// </summary>
        /// <remarks>
        /// GameId should be in the format `Season_Week_VisitorAbr_HomeTAbr`.
        /// For example:
        ///    - One game: `2020_17_DAL_NYG`
        ///    - Multiple games: `2020_01_DAL_LA,2020_02_ATL_DAL`
        /// </remarks>
        public string GameId { get; set; }

        public Dictionary<int, List<string>> GetGameIdsBySeason()
        {
            var gameIds = GameId.Split(",");
            var seasonGameIds = gameIds.Select(g => (g.Substring(0, 4), g)).Distinct();

            var gameIdsBySeason = seasonGameIds
                .GroupBy(x => StringParser.ToInt(x.Item1))
                .ToDictionary(x => x.Key, x => x.Select(x => x.g).ToList());

            return gameIdsBySeason;
        }

        public Dictionary<string, string[]> Validate()
        {
            var errors = new Dictionary<string, string[]>();

            var Today = DateTime.UtcNow;
            var currentSeason = Today.Month > 8 ? Today.Year : Today.Year - 1;

            if (Season > currentSeason || Season < 1999)
                errors["season"] = new[] {$"Season must be between 1999 and {currentSeason}"};

            if (Week < 1 || Week > 21)
                errors["week"] = new[]
                    {"Week must be between 1-17 (REG), Divisional (19), Conference (20) and Super Bowl (21)"};

            if (Team.Length < 2)
                errors["team"] = new[] {"Invalid team abbreviation given"};

            if (GameId == null)
            {
                if (!NonGameIdParameterSet())
                    errors["query"] = new[] {"If gameId is not used then one of Week, Season or Team must be provided"};
            }
            else
            {
                if (NonGameIdParameterSet())
                    errors["query"] = new[] {"If gameId is used then Week, Season and Team do not need to be provided"};
            }

            return errors;
        }

        private bool NonGameIdParameterSet()
        {
            return Week != null || Season != null || string.IsNullOrWhiteSpace(Team);
        }
    }
}