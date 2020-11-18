namespace FourthDown.Api.Parameters
{
    public class ScheduleQueryParameter
    {
        public int? Week { get; set; }
        public int? Season { get; set; }
        public string Team { get; set; }

        public bool IsNull() => Week == null && Season == null && string.IsNullOrWhiteSpace(Team);
    }
}