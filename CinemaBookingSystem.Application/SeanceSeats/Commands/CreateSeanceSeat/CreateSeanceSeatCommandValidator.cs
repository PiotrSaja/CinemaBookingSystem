using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CinemaBookingSystem.Application.SeanceSeats.Commands.CreateSeanceSeat
{
    public class CreateSeanceSeatCommandValidator : AbstractValidator<CreateSeanceSeatCommand>
    {
        public CreateSeanceSeatCommandValidator()
        {
            RuleFor(x => x.SeanceId)
                .NotNull();
            RuleFor(x => x.Price)
                .NotNull()
                .GreaterThan(0);
            RuleFor(x => x.CinemaSeatId)
                .NotNull();
        }
    }
}
