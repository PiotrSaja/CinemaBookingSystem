﻿using System.Collections.Generic;
using MediatR;

namespace CinemaBookingSystem.Application.Bookings.Commands.CreateBooking
{
    public class CreateBookingCommand : IRequest<CreateBookingCommandHandler.Result>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public int SeanceId { get; set; }
        public List<int> SeanceSeatIds { get; set; }
    }
}
