using FluentValidation;

namespace CinemaBookingSystem.Application.CinemaSeats.Commands.CreateCinemaSeat
{
    public class CreateCinemaSeatCommandValidator : AbstractValidator<CreateCinemaSeatCommand>
    {
        #region CreateCinemaSeatCommandValidator()
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
        #endregion
    }
}
