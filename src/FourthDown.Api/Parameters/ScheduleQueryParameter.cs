using System;
using System.Collections.Generic;

namespace FourthDown.Api.Parameters
{
    /// <summary>
    /// A combination of `Season`, `Week` or `Team` can be provided.
    /// Default to the current season if not provided and current week if not provided.
    /// Returns all teams if not provided.
    /// </summary>
    public class ScheduleQueryParameter : QueryParameterBase
    {
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

            if (!string.IsNullOrWhiteSpace(Team) && Team.Length < 2)
                errors["team"] = new[] {"Invalid team abbreviation given"};

            return errors;
        }
    }
}