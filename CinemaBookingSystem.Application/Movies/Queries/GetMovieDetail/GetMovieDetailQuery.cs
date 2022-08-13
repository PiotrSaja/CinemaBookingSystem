﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace CinemaBookingSystem.Application.Movies.Queries.GetMovieDetail
{
    public class GetMovieDetailQuery : IRequest<MovieDetailVm>
    {
        public int MovieId { get; set; }
    }
}
