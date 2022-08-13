using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CinemaBookingSystem.Application.Bookings.Commands.CreateBooking
{
    public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
    {
        public CreateBookingCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(128);
            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(128);
            RuleFor(x => x.PhoneNumber)
                .NotEmpty();
            RuleFor(x => x.SeanceId)
                .NotNull();
            RuleFor(x => x.SeanceSeatIds)
                .NotEmpty();
        }
    }
}
