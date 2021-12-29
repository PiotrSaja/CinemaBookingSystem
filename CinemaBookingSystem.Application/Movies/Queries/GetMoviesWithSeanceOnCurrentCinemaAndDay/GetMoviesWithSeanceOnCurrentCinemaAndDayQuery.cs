﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace CinemaBookingSystem.Application.Movies.Queries.GetMoviesWithSeanceOnCurrentCinemaAndDay
{
    public class GetMoviesWithSeanceOnCurrentCinemaAndDayQuery : IRequest<MoviesVm>
    {
        public int CinemaId { get; set; }
        public DateTime Date { get; set; }
    }
}
