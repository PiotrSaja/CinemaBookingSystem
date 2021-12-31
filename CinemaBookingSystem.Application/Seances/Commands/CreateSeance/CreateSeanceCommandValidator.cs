using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CinemaBookingSystem.Application.Seances.Commands.CreateSeance
{
    public class CreateSeanceCommandValidator : AbstractValidator<CreateSeanceCommand>
    {
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
    }
}
