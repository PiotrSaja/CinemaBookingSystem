﻿using MediatR;

namespace CinemaBookingSystem.Application.SeanceSeats.Commands.LockSeanceSeat
{
    public class LockSeanceSeatCommand : IRequest<bool>
    {
        public int SeanceSeatId { get; set; }
    }
}
