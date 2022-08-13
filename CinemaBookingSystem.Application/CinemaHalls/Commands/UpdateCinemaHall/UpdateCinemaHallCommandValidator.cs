using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CinemaBookingSystem.Application.CinemaHalls.Commands.UpdateCinemaHall
{
    public class UpdateCinemaHallCommandValidator : AbstractValidator<UpdateCinemaHallCommand>
    {
        public UpdateCinemaHallCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(128);
            RuleFor(x => x.TotalSeats)
                .NotNull()
                .GreaterThanOrEqualTo(0);
            RuleFor(x => x.CinemaId)
                .NotNull();
            RuleFor(x => x.NumberOfColumns)
                .NotNull();
            RuleFor(x => x.NumberOfRows)
                .NotNull();
        }
    }
}
