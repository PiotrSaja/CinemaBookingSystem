using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CinemaBookingSystem.Application.CinemaHalls.Commands.CreateCinemaHall
{
    public class CreateCinemaHallCommandValidator : AbstractValidator<CreateCinemaHallCommand>
    {
        public CreateCinemaHallCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(128);
            RuleFor(x => x.TotalSeats)
                .NotNull()
                .GreaterThanOrEqualTo(0);
            RuleFor(x => x.CinemaId)
                .NotNull();
        }
    }
}
