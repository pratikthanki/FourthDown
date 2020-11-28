namespace FourthDown.Api.Parameters
{
    public class QueryParameterBase
    {
        /// <summary>
        /// Game week;
        /// - REG (1-17)
        /// - Wild Card (18)
        /// - Divisional (19)
        /// - Conference (20)
        /// - Super Bowl (21)
        /// </summary>
        public int? Week { get; set; }

        /// <summary>
        /// Seasons between 1999 and current season
        /// </summary>
        public int? Season { get; set; }

        /// <summary>
        /// Team abbreviation. See /api/teams for abr attribute
        /// </summary>
        public string Team { get; set; }
    }
}