using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using CinemaBookingSystem.Application.Common.Exceptions;
using CinemaBookingSystem.Application.Common.Extensions;
using CinemaBookingSystem.Application.Common.Interfaces;
using CinemaBookingSystem.Domain.Entities;

namespace CinemaBookingSystem.Application.Movies.Queries.GetMoviesDaysToPremiere
{
    public class GetMoviesPredictionQueryHandler : IRequestHandler<GetMoviesDaysToPremiereQuery, MoviesDetailVm>
    {
        private readonly ICinemaDbContext _context;
        private readonly IMapper _mapper;
        private readonly IDateTime _dateTime;

        public GetMoviesPredictionQueryHandler(ICinemaDbContext context, IMapper mapper, IDateTime dateTime)
        {
            _context = context;
            _mapper = mapper;
            _dateTime = dateTime;
        }

        public async Task<MoviesDetailVm> Handle(GetMoviesDaysToPremiereQuery request, CancellationToken cancellationToken)
        {
            if (request.PageSize < 1 && request.PageIndex < 1) { throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, "Page size and Page index can't be null or less than 1"); }
            if (request.PageSize < 1) { throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, "Page size can't be null or less than 1"); }
            if (request.PageIndex < 1) { throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, "Page index can't be null or less than 1"); }

            var dataTimeNow = _dateTime.Now.Date;
            var movies = await _context.Movies
                .Where(x => x.StatusId != 0 && EF.Functions.DateDiffDay(dataTimeNow, x.Released) >= request.DaysToPremiere)
                .AsNoTracking()
                .OrderByDescending(p => p.Released)
                .PaginateAsync(request.PageIndex, request.PageSize, cancellationToken);

            if (request.DaysToPremiere < 0)
            {
                movies = await _context.Movies
                    .Where(x => x.StatusId != 0 && EF.Functions.DateDiffDay(dataTimeNow, x.Released) <= 0)
                    .AsNoTracking()
                    .OrderByDescending(p => p.Released)
                    .PaginateAsync(request.PageIndex, request.PageSize, cancellationToken);
            }


            if (movies == null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "Not exists records in database");
            }

            var moviesDto = _mapper.Map<List<Movie>, List<MovieDetailDto>>(movies.Items.ToList());

            var moviesDetailVm = new MoviesDetailVm()
            {
                CurrentPage = movies.CurrentPage,
                TotalItems = movies.TotalItems,
                TotalPages = movies.TotalPages,
                Items = moviesDto
            };

            return moviesDetailVm;
        }
    }
}
