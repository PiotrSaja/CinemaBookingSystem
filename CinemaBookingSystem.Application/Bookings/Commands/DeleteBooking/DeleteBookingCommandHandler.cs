using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CinemaBookingSystem.Application.Common.Exceptions;
using CinemaBookingSystem.Application.Common.Interfaces;
using CinemaBookingSystem.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingSystem.Application.Bookings.Commands.DeleteBooking
{
    public class DeleteBookingCommandHandler : IRequestHandler<DeleteBookingCommand, Unit>
    {
        private readonly ICinemaDbContext _context;

        public DeleteBookingCommandHandler(ICinemaDbContext context)
        {
            _context = context;
        }
        public async Task<Unit> Handle(DeleteBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = await _context.Bookings.Where(x => x.Id == request.BookingId && x.SeanceId != 0)
                .Include(x => x.SeanceSeats)
                .FirstOrDefaultAsync(cancellationToken);
            if (booking == null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "Not exists in database, check your id");
            }

            ChangeStatusInShowSeat(booking.SeanceSeats, cancellationToken);

            _context.Bookings.Remove(booking);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
        private async void ChangeStatusInShowSeat(ICollection<SeanceSeat> items, CancellationToken cancellationToken)
        {
            foreach (SeanceSeat item in items)
            {
                item.SeatStatus = false;
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
