using FluentValidation;

namespace CinemaBookingSystem.Application.SeanceSeats.Commands.CreateSeanceSeat
{
    public class CreateSeanceSeatCommandValidator : AbstractValidator<CreateSeanceSeatCommand>
    {
        #region CreateSeanceSeatCommandValidator()
        public CreateSeanceSeatCommandValidator()
        {
            RuleFor(x => x.SeanceId)
                .NotNull();
            RuleFor(x => x.Price)
                .NotNull()
                .GreaterThan(0);
            RuleFor(x => x.CinemaSeatId)
                .NotNull();
        }
        #endregion
    }
}
