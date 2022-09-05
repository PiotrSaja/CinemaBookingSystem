using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CinemaBookingSystem.Application.Common.Exceptions;
using CinemaBookingSystem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingSystem.Application.SeanceSeats.Commands.LockSeanceSeat
{
    public class LockSeanceSeatCommandHandler : IRequestHandler<LockSeanceSeatCommand, bool>
    {
        private readonly IUserService _userService;
        private readonly ISeatLockingService _seatLockingService;
        private readonly IDateTime _dateTime;
        private readonly ICinemaDbContext _context;

        #region LockSeanceSeatCommandHandler()
        public LockSeanceSeatCommandHandler(IUserService userService, ISeatLockingService seatLockingService,
            IDateTime dateTime, ICinemaDbContext context)
        {
            _userService = userService;
            _seatLockingService = seatLockingService;
            _dateTime = dateTime;
            _context = context;
        }
        #endregion

        #region Handle()
        public async Task<bool> Handle(LockSeanceSeatCommand request, CancellationToken cancellationToken)
        {
            var seanceSeat = await _context.SeanceSeats
                .FirstOrDefaultAsync(x => x.Id == request.SeanceSeatId && x.StatusId != 0, cancellationToken);

            if (seanceSeat == null)
                throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, "Incorrect SeanceSeatId");

            var currentUserId = _userService.Id;
            var actualTime = _dateTime.Now;
            var expirationTime = actualTime.AddMinutes(10);

            var lockSeatComplete = await _seatLockingService.LockSeat(request.SeanceSeatId, currentUserId, expirationTime);

            return lockSeatComplete;
        }
        #endregion
    }
}
