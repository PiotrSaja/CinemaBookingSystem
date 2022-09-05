using FluentValidation;

namespace CinemaBookingSystem.Application.SeanceSeats.Commands.UpdateSeanceSeat
{
    public class UpdateSeanceSeatCommandValidator : AbstractValidator<UpdateSeanceSeatCommand>
    {
        #region UpdateSeanceSeatCommandValidator()
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
        #endregion
    }
}
