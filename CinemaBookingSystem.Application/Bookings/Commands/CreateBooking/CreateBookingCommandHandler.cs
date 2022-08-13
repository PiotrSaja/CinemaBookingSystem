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
using CinemaBookingSystem.Domain.Enums;
using CinemaBookingSystem.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingSystem.Application.Bookings.Commands.CreateBooking
{
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, int>
    {
        private readonly ICinemaDbContext _context;
        private readonly IUserService _userService;
        private readonly ISeatLockingService _seatLockingService;

        public CreateBookingCommandHandler(ICinemaDbContext context, IUserService userService, ISeatLockingService seatLockingService)
        {
            _context = context;
            _userService = userService;
            _seatLockingService = seatLockingService;
        }

        public async Task<int> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            var seance = await _context.Seances.Where(x => x.Id == request.SeanceId).FirstOrDefaultAsync(cancellationToken);

            if (seance == null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, "Not exists seance in database, check your seanceId");
            }

            var seanceSeats = _context.SeanceSeats.Where(x => request.SeanceSeatIds.Contains(x.Id)).ToList();

            if (seanceSeats.Count == 0)
            {
                throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, "Seance seat ids can't be empty");
            }
            else
            {
                var blockedSeatsList = _seatLockingService.LockedList;
                foreach (var itemSeat in seanceSeats)
                {
                    var existsInLockingService = blockedSeatsList.Exists(x => x.SeanceSeatId == itemSeat.Id && x.UserId == _userService.Id);
                    if (!existsInLockingService)
                    {
                        throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, "Seance seat not locked in service");
                    }
                }
            }

            ChangeStatusInShowSeat(seanceSeats, cancellationToken);

            var booking = new Booking()
            {
                PersonalName = new PersonalName()
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    PhoneNumber = request.PhoneNumber
                },
                SeanceId = request.SeanceId,
                NumberOfSeats = seanceSeats.Count(),


                UserId = _userService.Id,
                BookingStatus = BookingStatus.Successful,
                SeanceSeats = seanceSeats.ToList()
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync(cancellationToken);

            return booking.Id;
        }

        private async void ChangeStatusInShowSeat(List<SeanceSeat> items, CancellationToken cancellationToken)
        {
            foreach (var item in items)
            {
                item.SeatStatus = true;
                _context.SeanceSeats.Update(item);
            }
        }
    }
}
