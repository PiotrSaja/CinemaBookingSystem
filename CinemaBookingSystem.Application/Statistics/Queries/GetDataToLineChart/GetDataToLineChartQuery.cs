using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaBookingSystem.Domain.Common;
using CinemaBookingSystem.Domain.Entities;
using MediatR;

namespace CinemaBookingSystem.Application.Statistics.Queries.GetDataToLineChart
{
    public class GetDataToLineChartQuery : IRequest<LineChartDataModel>
    {
        public DateTime? DateTimeFrom { get; set; }
        public DateTime? DateTimeTo { get; set; }
        public int? Month { get; set; }
        public string DataType { get; set; }
    }
}
