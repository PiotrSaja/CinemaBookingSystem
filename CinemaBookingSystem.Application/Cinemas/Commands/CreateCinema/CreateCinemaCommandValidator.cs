using FluentValidation;

namespace CinemaBookingSystem.Application.Cinemas.Commands.CreateCinema
{
    public class CreateCinemaCommandValidator : AbstractValidator<CreateCinemaCommand>
    {
        #region CreateCinemaCommandValidator()
        public CreateCinemaCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(64);
            RuleFor(x => x.TotalCinemaHalls)
                .NotEmpty()
                .GreaterThanOrEqualTo(0);
            RuleFor(x => x.City)
                .NotEmpty()
                .MaximumLength(64);
            RuleFor(x => x.Country)
                .NotEmpty()
                .MaximumLength(64);
            RuleFor(x => x.State)
                .NotEmpty()
                .MaximumLength(64);
            RuleFor(x => x.Street)
                .NotEmpty()
                .MaximumLength(128);
            RuleFor(x => x.ZipCode)
                .NotEmpty()
                .MaximumLength(16);
        }
        #endregion
    }
}
