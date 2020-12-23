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
        /// <remarks>
        /// Note: only completed games for the current week are shown.
        /// </remarks>
        public int? Week { get; set; }

        /// <summary>
        /// Valid seasons from 2001 to the current season.
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
    }
}