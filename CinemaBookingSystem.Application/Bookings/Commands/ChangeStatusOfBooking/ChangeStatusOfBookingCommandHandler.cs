using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CinemaBookingSystem.Application.Common.Exceptions;
using CinemaBookingSystem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingSystem.Application.Bookings.Commands.ChangeStatusOfBooking
{
    public class ChangeStatusOfBookingCommandHandler : IRequestHandler<ChangeStatusOfBookingCommand, int>
    {
        private readonly ICinemaDbContext _context;

        #region ChangeStatusOfBookingCommandHandler()
        public ChangeStatusOfBookingCommandHandler(ICinemaDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Handle()
        public async Task<int> Handle(ChangeStatusOfBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = await _context.Bookings
                .Where(x => x.Id == request.BookingId)
                .FirstOrDefaultAsync(cancellationToken);

            if (booking == null)
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "Not exists in database, check your id");

            booking.BookingStatus = request.Status;

            await _context.SaveChangesAsync(cancellationToken);

            return booking.Id;
        }
        #endregion
    }
}
