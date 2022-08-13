using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CinemaBookingSystem.Application.CinemaSeats.Commands.CreateCinemaSeat
{
    public class CreateCinemaSeatCommandValidator : AbstractValidator<CreateCinemaSeatCommand>
    {
        public CreateCinemaSeatCommandValidator()
        {
            RuleFor(x => x.SeatNumber)
                .NotNull();
            RuleFor(x => x.Row)
                .NotNull();
            RuleFor(x => x.SeatType)
                .NotNull();
            RuleFor(x => x.CinemaHallId)
                .NotNull();
        }
    }
}
