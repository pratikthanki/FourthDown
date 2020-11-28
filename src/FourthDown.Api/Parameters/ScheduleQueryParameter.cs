using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FourthDown.Api.Parameters
{
    /// <summary>
    /// 
    /// </summary>
    public class ScheduleQueryParameter : QueryParameterBase
    {
#pragma warning disable 1591
        public bool IsNull() => Week == null && Season == null && string.IsNullOrWhiteSpace(Team);
#pragma warning restore 1591
    }
}