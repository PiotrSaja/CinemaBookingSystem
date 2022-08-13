using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CinemaBookingSystem.Application.Cinemas.Commands.UpdateCinema
{
    public class UpdateCinemaCommandValidator : AbstractValidator<UpdateCinemaCommand>
    {
        public UpdateCinemaCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotNull();
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(64);
            RuleFor(x => x.TotalCinemaHalls)
                .NotEmpty()
                .GreaterThanOrEqualTo(0);
            RuleFor(x => x.City)
                .NotEmpty()
                .MaximumLength(64);
            RuleFor(x => x.Country)
                .NotEmpty()
                .MaximumLength(64);
            RuleFor(x => x.State)
                .NotEmpty()
                .MaximumLength(64);
            RuleFor(x => x.Street)
                .NotEmpty()
                .MaximumLength(128);
            RuleFor(x => x.ZipCode)
                .NotEmpty()
                .MaximumLength(16);
        }
    }
}
