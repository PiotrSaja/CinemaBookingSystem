using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CinemaBookingSystem.Application.SeanceSeats.Commands.UpdateSeanceSeat
{
    public class UpdateSeanceSeatCommandValidator : AbstractValidator<UpdateSeanceSeatCommand>
    {
        public UpdateSeanceSeatCommandValidator()
        {
            RuleFor(x => x.SeanceId)
                .NotNull();
            RuleFor(x => x.Price)
                .NotNull()
                .GreaterThan(0);
            RuleFor(x => x.CinemaSeatId)
                .NotNull();
            RuleFor(x => x.SeatStatus)
                .NotNull();
        }
    }
}
