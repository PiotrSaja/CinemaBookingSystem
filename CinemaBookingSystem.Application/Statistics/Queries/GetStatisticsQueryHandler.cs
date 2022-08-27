using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CinemaBookingSystem.Application.Common.Interfaces;
using CinemaBookingSystem.Domain.Enums;
using MediatR;

namespace CinemaBookingSystem.Application.Statistics.Queries
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

            var bookings = await _context.Bookings.Include(x=>x.SeanceSeats).ToListAsync(cancellationToken);
            var seances =  await _context.Seances.ToListAsync(cancellationToken);

            result.NumberOfBookings = bookings.Count(x => x.BookingStatus == BookingStatus.Successful);
            result.NumberOfTodayBookings = bookings.Count(x => x.Created == DateTime.Today);
            result.NumberOfCustomers = bookings.Count();
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

            result.BookingsInMonths = new List<int>();

            for (int i = 1; i <= 12; i++)
                result.BookingsInMonths.Add(bookings.Count(x => x.Created.Month == i));


            result.RevenueInMonths = new List<double>();

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
                result.RevenueInMonths.Add(temp);
            }

            return result;
        }
        #endregion
    }
}
