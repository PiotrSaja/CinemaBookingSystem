using MediatR;
using System;

namespace CinemaBookingSystem.Application.Statistics.Queries.GetStatistics
{
    public class GetStatisticsQuery : IRequest<StatisticsModel>
    {
        public DateTime? DateTimeFrom { get; set; }
        public DateTime? DateTimeTo { get; set; }
        public int? Month { get; set; }
    }
}
