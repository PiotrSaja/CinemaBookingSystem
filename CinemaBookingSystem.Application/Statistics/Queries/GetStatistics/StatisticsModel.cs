namespace CinemaBookingSystem.Application.Statistics.Queries.GetStatistics
{
    public class StatisticsModel
    {
        public int NumberOfBookings { get; set; }
        public int NumberOfTodayBookings { get; set; }
        public int NumberOfSeances { get; set; }
        public int NumberOfTodaySeances { get; set; }
        public int NumberOfCustomers { get; set; }
        public double RevenueAll { get; set; }
        public double RevenueMinusOneWeek { get; set; }
    }
}
