using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CinemaBookingSystem.Application.Seances.Commands.UpdateSeance
{
    public class UpdateSeanceCommandValidator : AbstractValidator<UpdateSeanceCommand>
    {
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
    }
}
