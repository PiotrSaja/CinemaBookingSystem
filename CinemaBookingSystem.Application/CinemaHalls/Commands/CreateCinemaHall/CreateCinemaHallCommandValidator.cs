using FluentValidation;

namespace CinemaBookingSystem.Application.CinemaHalls.Commands.CreateCinemaHall
{
    public class CreateCinemaHallCommandValidator : AbstractValidator<CreateCinemaHallCommand>
    {
        #region CreateCinemaHallCommandValidator()
        public CreateCinemaHallCommandValidator()
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
        #endregion
    }
}
