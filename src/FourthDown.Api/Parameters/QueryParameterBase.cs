using System;
using System.Collections.Generic;

namespace FourthDown.Api.Parameters
{
    public class QueryParameterBase
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
        public int? Week { get; set; }

        /// <summary>
        /// Valid seasons from 1999 to the current season.
        /// </summary>
        public int? Season { get; set; }

        /// <summary>
        /// Team abbreviation. See `/api/teams` for the `abr` attribute.
        /// </summary>
        public string Team { get; set; }

        internal bool NonGameIdParameterSet()
        {
            return Week != null || Season != null || Team != null;
        }

#pragma warning disable 1591
        public Dictionary<string, string[]> ValidateBase()
        {
            var errors = new Dictionary<string, string[]>();

            var Today = DateTime.UtcNow;
            var currentSeason = Today.Month > 8 ? Today.Year : Today.Year - 1;

            if (Season > currentSeason || Season < 1999)
                errors["season"] = new[] {$"Season must be between 1999 and {currentSeason}"};

            if (Week < 1 || Week > 21)
                errors["week"] = new[]
                    {"week must be between 1-17 (REG), Divisional (19), Conference (20) and Super Bowl (21)"};

            if (!string.IsNullOrWhiteSpace(Team) && Team.Length < 2)
                errors["team"] = new[] {"Invalid team abbreviation given"};

            return errors;
        }
#pragma warning restore 1591
    }
}