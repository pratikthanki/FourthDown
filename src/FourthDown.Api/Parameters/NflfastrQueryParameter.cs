using System;
using System.Collections.Generic;

namespace FourthDown.Api.Parameters
{
    public class NflfastrQueryParameter : QueryParameterBase
    {
        public Dictionary<string, string[]> Validate()
        {
            var errors = new Dictionary<string, string[]>();

            var Today = DateTime.UtcNow;
            var currentSeason = Today.Month > 8 ? Today.Year : Today.Year - 1;

            if (Season > currentSeason || Season < 2001)
                errors["season"] = new[] {$"Season must be between 2001 and {currentSeason}"};

            if (Week == null && string.IsNullOrWhiteSpace(Team))
                errors["requiredField"] = new[] {$"One of Week or team must be provided."};

            return errors;
        }
    }
}