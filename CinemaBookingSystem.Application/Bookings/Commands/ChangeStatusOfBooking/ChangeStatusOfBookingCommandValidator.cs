using FluentValidation;

namespace CinemaBookingSystem.Application.Bookings.Commands.ChangeStatusOfBooking
{
    public class ChangeStatusOfBookingCommandValidator : AbstractValidator<ChangeStatusOfBookingCommand>
    {
        public ChangeStatusOfBookingCommandValidator()
        {
            RuleFor(x => x.Status)
                .NotNull();
            RuleFor(x => x.BookingId)
                .NotNull();
        }
    }
}
