using FluentValidation;

namespace CinemaBookingSystem.Application.Seances.Commands.UpdateSeance
{
    public class UpdateSeanceCommandValidator : AbstractValidator<UpdateSeanceCommand>
    {
        #region UpdateSeanceCommandValidator()
        public UpdateSeanceCommandValidator()
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
