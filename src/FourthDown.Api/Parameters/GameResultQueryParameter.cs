using System;
using System.Collections.Generic;

namespace FourthDown.Api.Parameters
{
    /// <summary>
    /// Filters to get all games between two sets of teams.
    /// Limit by season phase and offset.
    /// </summary>
    public class GameResultQueryParameter
    {
        /// <summary>
        /// Team abbreviation.
        /// </summary>
        /// <example>
        /// DAL
        /// </example>
        public string Team { get; set; }

        /// <summary>
        /// Opposition team abbreviation.
        /// </summary>
        /// <example>
        /// PHI
        /// </example>
        public string Opposition { get; set; }

        /// <summary>
        /// Last X number of games for the Team specified.
        /// Or just games that involved the Opposition given.
        /// </summary>
        /// <example>
        /// 10
        /// </example>
        public int GameOffset { get; set; }

        /// <summary>
        /// Select between season phase to get all prior games between teams.
        /// One of:
        /// ```
        /// - Reg
        /// - Post (which includes; WC, DIV, CON, SB)
        /// - All
        /// ```
        /// </summary>
        /// <example>
        /// Post
        /// </example>
        public string GameType { get; set; }

        public Dictionary<string, string[]> Validate()
        {
            var errors = new Dictionary<string, string[]>();

            if (GameOffset < 1)
                errors["gameOffset"] = new[] {"GameOffset should be greater than 1."};
            
            if (string.IsNullOrWhiteSpace(Team))
                errors["team"] = new[] {"Team must be provided."};
            
            if (!Enum.TryParse(GameType, out GameTypeFilter _))
                errors["team"] = new[] {"GameType should be one of; 'Reg', 'Post' or 'All'."};

            return errors;
        }

        public GameTypeFilter ToGameTypeFilter() => Enum.Parse<GameTypeFilter>(GameType);

    }

    public enum GameTypeFilter
    {
        Reg,
        Post,
        All
    }
}