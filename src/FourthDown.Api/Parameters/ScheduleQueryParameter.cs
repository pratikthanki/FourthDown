
namespace FourthDown.Api.Parameters
{
    /// <summary>
    /// A combination of Season, Week and Team can be provided.
    /// When an option is not given, it will default to the current Season and/or Week.
    /// </summary>
    public class ScheduleQueryParameter : QueryParameterBase
    {
#pragma warning disable 1591
        public bool IsNull() => Week == null && Season == null && string.IsNullOrWhiteSpace(Team);
#pragma warning restore 1591
    }
}