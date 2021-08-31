using System;
using System.Collections.Generic;
using FourthDown.Shared.Utilities;

namespace FourthDown.Api.Parameters
{
    public class NflfastrQueryParameter
    {
        /// <summary>
        /// Valid seasons from 2001 to the current season.
        /// </summary>
        /// <remarks>
        /// Defaults to the current season if not set.
        /// </remarks>
        /// <example>
        /// `2018`
        /// </example>
        public int? Season { get; set; }

        /// <summary>
        /// Optional. Team abbreviation. See `/api/teams` for the `abr` attribute.
        /// </summary>
        /// <remarks>
        /// Returns data for all teams if not set.
        /// </remarks>
        /// <example>
        /// `DAL`
        /// </example>
        public string Team { get; set; }

        public Dictionary<string, string[]> Validate()
        {
            var errors = new Dictionary<string, string[]>();

            var currentSeason = StringParser.GetCurrentSeason();

            if (Season > currentSeason || Season < 1999)
                errors["season"] = new[] {$"Season must be between 2001 and {currentSeason}"};

            return errors;
        }
    }
}