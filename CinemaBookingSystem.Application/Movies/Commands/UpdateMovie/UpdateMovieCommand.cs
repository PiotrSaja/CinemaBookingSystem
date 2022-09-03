using System;
using System.Collections.Generic;
using MediatR;

namespace CinemaBookingSystem.Application.Movies.Commands.UpdateMovie
{
    public class UpdateMovieCommand : IRequest<int>
    {
        public int MovieId { get; set; }

        public string Title { get; set; }
        public DateTime Released { get; set; }
        public int Duration { get; set; }
        public List<int> Genres { get; set; }
        public int DirectorId { get; set; }
        public List<int> Actors { get; set; }
        public string Plot { get; set; }
        public string Country { get; set; }
        public string Language { get; set; }
        public string PosterPath { get; set; }
        public string ImdbRating { get; set; }
    }
}
