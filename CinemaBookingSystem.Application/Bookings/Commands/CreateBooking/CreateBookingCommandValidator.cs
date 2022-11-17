using FluentValidation;

namespace CinemaBookingSystem.Application.Bookings.Commands.CreateBooking
{
    public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
    {
        public CreateBookingCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(40)
                .MinimumLength(2);
            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(40)
                .MinimumLength(2);
            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .Matches("^([+]?[\\s0-9]+)?(\\d{3}|[(]?[0-9]+[)])?([-]?[\\s]?[0-9])+$");
            RuleFor(x => x.SeanceId)
                .NotNull();
            RuleFor(x => x.SeanceSeatIds)
                .NotEmpty();
        }
    }
}
