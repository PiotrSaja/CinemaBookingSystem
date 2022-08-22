using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using CinemaBookingSystem.Application.Common.Exceptions;
using CinemaBookingSystem.Application.Common.Extensions;
using CinemaBookingSystem.Application.Common.Interfaces;
using CinemaBookingSystem.Domain.Entities;
using CinemaBookingSystem.Domain.Enums;

namespace CinemaBookingSystem.Application.Movies.Queries.GetMoviesPrediction
{
    public class GetMoviesPredictionQueryHandler : IRequestHandler<GetMoviesPredictionQuery, MoviesDetailVm>
    {
        private readonly ICinemaDbContext _context;
        private readonly IMapper _mapper;
        private readonly IDateTime _dateTime;
        private readonly IUserVoteService _userVoteService;
        private readonly IUserService _userService;

        public GetMoviesPredictionQueryHandler(ICinemaDbContext context, IMapper mapper, IDateTime dateTime, IUserVoteService userVoteService, IUserService userService)
        {
            _context = context;
            _mapper = mapper;
            _dateTime = dateTime;
            _userVoteService = userVoteService;
            _userService = userService;
        }

        public async Task<MoviesDetailVm> Handle(GetMoviesPredictionQuery request, CancellationToken cancellationToken)
        {
            if (request.PageSize < 1 && request.PageIndex < 1) { throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, "Page size and Page index can't be null or less than 1"); }
            if (request.PageSize < 1) { throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, "Page size can't be null or less than 1"); }
            if (request.PageIndex < 1) { throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, "Page index can't be null or less than 1"); }


            var result = await _userVoteService.GetPredictions(_userService.Id, cancellationToken);

            var movieResultAssigns = result.Where(x=>x.Result > 0).OrderByDescending(x => x.Result).Select(x=>x.MovieId).ToList();

            var watchedFilms = await _context.Bookings
                .Where(x =>
                    x.UserId == _userService.Id && 
                    x.BookingStatus == BookingStatus.Successful)
                .Include(x=>x.Seance)
                .Select(x=>x.Seance.MovieId)
                .ToListAsync(cancellationToken);

            watchedFilms.ForEach(x =>
            {
                movieResultAssigns.Remove(x);
            });

            var movies = await _context.Movies
                .Where(x => x.StatusId != 0 && movieResultAssigns.Contains(x.Id))
                .AsNoTracking()
                .PaginateAsync(request.PageIndex, request.PageSize, cancellationToken);

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
                    