﻿using CinemaBookingSystem.Domain.Enums;
using MediatR;

namespace CinemaBookingSystem.Application.CinemaSeats.Commands.CreateCinemaSeat
{
    public class CreateCinemaSeatCommand : IRequest<int>
    {
        public int SeatNumber { get; set; }
        public int Row { get; set; }
        public SeatType SeatType { get; set; }
        public int CinemaHallId { get; set; }
    }
}
