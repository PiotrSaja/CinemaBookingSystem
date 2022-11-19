using CinemaBookingSystem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CinemaBookingSystem.Application.Bookings.Queries.GetBookings;
using CinemaBookingSystem.Domain.Entities;
using LinqKit;

namespace CinemaBookingSystem.Application.Statistics.Queries.GetDataToLineChart
{
    public class GetDataToLineChartQueryHandler : IRequestHandler<GetDataToLineChartQuery, LineChartDataModel>
    {
        private readonly ICinemaDbContext _context;
        private readonly IMapper _mapper;

        #region GetDataToLineChartQueryHandler()

        public GetDataToLineChartQueryHandler(ICinemaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #endregion

        public async Task<LineChartDataModel> Handle(GetDataToLineChartQuery request, CancellationToken cancellationToken)
        {
            var result = new LineChartDataModel();
            result.DataToChart = new List<double>();

            var predictionBooking = PredicateBuilder.New<Booking>(true);

            predictionBooking.And(x => x.StatusId != 0);

            if (request.DateTimeFrom != null)
                predictionBooking.And(x=> x.Created >= request.DateTimeFrom);
            if (request.DateTimeTo != null)
                predictionBooking.And(x => x.Created <= request.DateTimeTo);
            if (request.Month != null)
                predictionBooking.And(x => x.Created.Month == request.Month);


            if (request.DataType == "Bookings")
            {
                var bookings = await _context.Bookings
                    .Include(x=>x.Seance)
                    .ThenInclude(x=>x.Movie)
                    .Where(predictionBooking)
                    .ToListAsync(cancellationToken);
                
                for (int i = 1; i <= 12; i++)
                    result.DataToChart.Add(bookings.Count(x => x.Created.Month == i));

                var bookingsDto = _mapper.Map<List<Booking>, List<BookingDto>>(bookings.ToList());
                result.Bookings = bookingsDto;
            }

            if (request.DataType == "Revenue")
            {
                var bookings = await _context.Bookings
                    .Include(x=>x.SeanceSeats)
                    .Where(predictionBooking)
                    .ToListAsync(cancellationToken);

                for (int i = 1; i <= 12; i++)
                {
                    var temp = 0.0d;
                    foreach (var booking in bookings.Where(x => x.Created.Month == i))
                    {

                        if (booking.SeanceSeats != null)
                        {
                            foreach (var seanceSeat in booking.SeanceSeats)
                            {
                                temp += seanceSeat.Price;
                            }
                        }
                    }
                    result.DataToChart.Add(temp);
                }
                var bookingsDto = _mapper.Map<List<Booking>, List<BookingDto>>(bookings.ToList());
                result.Bookings = bookingsDto;
            }
            if (request.DataType == "BookingsInMonth")
            {
                var bookings = await _context.Bookings
                    .Include(x => x.Seance)
                    .ThenInclude(x => x.Movie)
                    .Where(predictionBooking)
                    .ToListAsync(cancellationToken);

                for (int i = 1; i <= 31; i++)
                    result.DataToChart.Add(bookings.Count(x => x.Created.Day == i));

                var bookingsDto = _mapper.Map<List<Booking>, List<BookingDto>>(bookings.ToList());
                result.Bookings = bookingsDto;
            }

            if (request.DataType == "RevenueInMonth")
            {
                var bookings = await _context.Bookings
                    .Include(x => x.SeanceSeats)
                    .Where(predictionBooking)
                    .ToListAsync(cancellationToken);

                for (int i = 1; i <= 31; i++)
                {
                    var temp = 0.0d;
                    foreach (var booking in bookings.Where(x => x.Created.Day == i))
                    {

                        if (booking.SeanceSeats != null)
                        {
                            foreach (var seanceSeat in booking.SeanceSeats)
                            {
                                temp += seanceSeat.Price;
                            }
                        }
                    }
                    result.DataToChart.Add(temp);
                }
                var bookingsDto = _mapper.Map<List<Booking>, List<BookingDto>>(bookings.ToList());
                result.Bookings = bookingsDto;
            }

            return result;
        }
    }
}
