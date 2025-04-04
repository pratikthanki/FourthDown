using System.Collections.Generic;
using FourthDown.Shared.Utilities;

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

            var currentSeason = StringParser.GetCurrentSeason();

            if (Season > currentSeason || Season < 1999)
                errors["season"] = new[] {$"Season must be between 2001 and {currentSeason}"};

            if (!string.IsNullOrWhiteSpace(Team) && Team.Length < 2)
                errors["team"] = new[] {"Invalid team abbreviation given"};

            return errors;
        }
    }
}