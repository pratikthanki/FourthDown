using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FourthDown.Api.Parameters
{
    /// <summary>
    /// 
    /// </summary>
    public class ScheduleQueryParameter
    {
        /// <summary>
        /// 
        /// </summary>
        public int? Week { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? Season { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Team { get; set; }

#pragma warning disable 1591
        public bool IsNull() => Week == null && Season == null && string.IsNullOrWhiteSpace(Team);
#pragma warning restore 1591
    }
}