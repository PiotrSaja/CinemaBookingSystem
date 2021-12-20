using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace CinemaBookingSystem.Application.Movies.Commands.DeleteMovie
{
    public class DeleteMovieCommand : IRequest
    {
        public int MovieId { get; set; }
    }
}
