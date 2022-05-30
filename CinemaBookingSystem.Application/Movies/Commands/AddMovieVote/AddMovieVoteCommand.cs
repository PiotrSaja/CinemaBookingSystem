using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace CinemaBookingSystem.Application.Movies.Commands.AddMovieVote
{
    public class AddMovieVoteCommand : IRequest<int>
    {
        public int MovieId { get; set; }
        public double Vote { get; set; }
    }
}
