using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
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

        public LockSeanceSeatCommandHandler(IUserService userService, ISeatLockingService seatLockingService,
            IDateTime dateTime, ICinemaDbContext context)
        {
            _userService = userService;
            _seatLockingService = seatLockingService;
            _dateTime = dateTime;
            _context = context;
        }

        public async Task<bool> Handle(LockSeanceSeatCommand request, CancellationToken cancellationToken)
        {
            var seanceSeat = await _context.SeanceSeats
                .Where(x => x.Id == request.SeanceSeatId && x.StatusId != 0)
                .FirstOrDefaultAsync(cancellationToken);

            if (seanceSeat == null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, "Incorrect SeanceSeatId");
            }

            var currentUserId = _userService.Id;
            var actualTime = _dateTime.Now;
            var expirationTime = actualTime.AddMinutes(1);

            var lockSeatComplete = await _seatLockingService.LockSeat(request.SeanceSeatId, currentUserId, expirationTime);

            return lockSeatComplete;
        }
    }
}
