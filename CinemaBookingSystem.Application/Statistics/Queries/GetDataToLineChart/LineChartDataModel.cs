using System.Collections.Generic;
using CinemaBookingSystem.Application.Bookings.Queries.GetBookings;
using CinemaBookingSystem.Domain.Common;
using CinemaBookingSystem.Domain.Entities;

namespace CinemaBookingSystem.Application.Statistics.Queries.GetDataToLineChart
{
    public class LineChartDataModel
    {
        public List<double> DataToChart { get; set; }
        public List<BookingDto> Bookings { get; set; }
    }
}
    