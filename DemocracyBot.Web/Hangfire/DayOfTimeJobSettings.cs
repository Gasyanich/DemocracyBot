namespace DemocracyBot.Web.Hangfire
{
    public class DayOfTimeJobSettings
    {
        public int Offset { get; set; }

        public int DayNotification { get; set; }

        public int NightNotification { get; set; }
    }
}