using FluentValidation;

namespace CinemaBookingSystem.Application.Movies.Commands.UpdateMovie
{
    public class UpdateMovieCommandValidator : AbstractValidator<UpdateMovieCommand>
    {
        #region UpdateMovieCommandValidator()
        public UpdateMovieCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty();
            RuleFor(x => x.Released)
                .NotEmpty();
            RuleFor(x => x.Duration)
                .GreaterThanOrEqualTo(0);
            RuleFor(x => x.Genres)
                .NotEmpty();
            RuleFor(x => x.Actors)
                .NotEmpty();
            RuleFor(x => x.DirectorId)
                .NotNull();
            RuleFor(x => x.Plot)
                .NotEmpty();
            RuleFor(x => x.Country)
                .NotEmpty();
            RuleFor(x => x.Language)
                .NotEmpty();
            RuleFor(x => x.PosterPath)
                .NotEmpty();
            RuleFor(x => x.ImdbRating)
                .NotEmpty();
        }
        #endregion
    }
}
