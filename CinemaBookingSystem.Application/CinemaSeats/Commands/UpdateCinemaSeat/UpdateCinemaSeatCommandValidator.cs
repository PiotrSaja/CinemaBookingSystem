using FluentValidation;

namespace CinemaBookingSystem.Application.CinemaSeats.Commands.UpdateCinemaSeat
{
    public class UpdateCinemaSeatCommandValidator : AbstractValidator<UpdateCinemaSeatCommand>
    {
        #region UpdateCinemaSeatCommandValidator()
        public UpdateCinemaSeatCommandValidator()
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
