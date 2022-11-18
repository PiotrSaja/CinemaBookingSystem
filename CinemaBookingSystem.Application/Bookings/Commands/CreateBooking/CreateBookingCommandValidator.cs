using System.Text.RegularExpressions;
using FluentValidation;

namespace CinemaBookingSystem.Application.Bookings.Commands.CreateBooking
{
    public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
    {
        public CreateBookingCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MinimumLength(3).WithMessage("First name must be more than 2 characters.")
                .MaximumLength(128).WithMessage("First name must be less than 128 characters.");
            RuleFor(x => x.LastName)
                .NotEmpty()
                .MinimumLength(3).WithMessage("Last name must be more than 2 characters.")
                .MaximumLength(128).WithMessage("Last name must be less than 128 characters.");
            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .Matches(new Regex(@"(?:[0-9]\-?){6,14}[0-9]$"))
                .WithMessage("Not valid phone number.");
            RuleFor(x => x.SeanceId)
                .NotNull();
            RuleFor(x => x.SeanceSeatIds)
                .NotEmpty();
        }
    }
}
