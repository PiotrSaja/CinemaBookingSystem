using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CinemaBookingSystem.Application.Common.Interfaces;
using CinemaBookingSystem.Domain.Entities;
using CinemaBookingSystem.Domain.Enums;
using LinqKit;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingSystem.Application.Statistics.Queries.GetStatistics
{
    public class GetStatisticsQueryHandler : IRequestHandler<GetStatisticsQuery, StatisticsModel>
    {
        private readonly ICinemaDbContext _context;

        #region GetStatisticsQueryHandler()
        public GetStatisticsQueryHandler(ICinemaDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Handle()
        public async Task<StatisticsModel> Handle(GetStatisticsQuery request, CancellationToken cancellationToken)
        {
            var result = new StatisticsModel();

            var predictionBooking = PredicateBuilder.New<Booking>(true);

            predictionBooking.And(x => x.StatusId != 0 && x.BookingStatus == BookingStatus.Successful);

            if (request.DateTimeFrom != null)
                predictionBooking.And(x => x.Created >= request.DateTimeFrom);
            if (request.DateTimeTo != null)
                predictionBooking.And(x => x.Created <= request.DateTimeTo);
            if (request.Month != null)
                predictionBooking.And(x => x.Created.Month == request.Month);

            var predictionSeances = PredicateBuilder.New<Seance>(true);

            predictionSeances.And(x => x.StatusId != 0 && x.SeanceSeats.Count > 0);

            if (request.DateTimeFrom != null)
                predictionSeances.And(x => x.Created >= request.DateTimeFrom);
            if (request.DateTimeTo != null)
                predictionSeances.And(x => x.Created <= request.DateTimeTo);
            if (request.Month != null)
                predictionSeances.And(x => x.Created.Month == request.Month);

            var bookings = await _context.Bookings.Include(x=>x.SeanceSeats).Where(predictionBooking).ToListAsync(cancellationToken);
            var seances =  await _context.Seances.Where(predictionSeances).ToListAsync(cancellationToken);

            result.NumberOfBookings = bookings.Count(x => x.BookingStatus == BookingStatus.Successful);
            result.NumberOfTodayBookings = bookings.Count(x => x.Created == DateTime.Today);
            result.NumberOfCustomers = bookings.Select(x=>x.UserId).Distinct().Count();
            result.NumberOfSeances = seances.Count();
            result.NumberOfTodaySeances = seances.Count(x => x.Date == DateTime.Today);

            foreach (var booking in bookings)
            {
                if (booking.SeanceSeats != null)
                {
                    foreach (var seanceSeat in booking.SeanceSeats)
                    {
                        result.RevenueAll += seanceSeat.Price;
                    }
                }
            }

            foreach (var booking in bookings.Where(x=>x.Created < DateTime.Today.AddDays(-7)))
            {
                if (booking.SeanceSeats != null)
                {
                    foreach (var seanceSeat in booking.SeanceSeats)
                    {
                        result.RevenueMinusOneWeek += seanceSeat.Price;
                    }
                }
            }

            return result;
        }
        #endregion
    }
}
