using FluentValidation;

namespace CinemaBookingSystem.Application.Seances.Commands.CreateSeance
{
    public class CreateSeanceCommandValidator : AbstractValidator<CreateSeanceCommand>
    {
        #region CreateSeanceCommandValidator()
        public CreateSeanceCommandValidator()
        {
            RuleFor(x => x.Date)
                .NotNull();
            RuleFor(x => x.SeanceType)
                .NotNull();
            RuleFor(x => x.CinemaHallId)
                .NotNull();
            RuleFor(x => x.MovieId)
                .NotNull();
        }
        #endregion
    }
}
