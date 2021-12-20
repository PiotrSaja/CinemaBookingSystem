using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CinemaBookingSystem.Application.Common.Extensions;
using CinemaBookingSystem.Application.Common.Interfaces;
using CinemaBookingSystem.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingSystem.Application.Movies.Queries.GetMovies
{
    public class GetMoviesQueryHandler : IRequestHandler<GetMoviesQuery,MoviesVm>
    {
        private readonly ICinemaDbContext _context;
        private readonly IMapper _mapper;

        public GetMoviesQueryHandler(ICinemaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<MoviesVm> Handle(GetMoviesQuery request, CancellationToken cancellationToken)
        {
            var movies = await _context.Movies
                .Where(x => x.StatusId != 0)
                .Include(x => x.Genres)
                .AsNoTracking()
                .PaginateAsync(request.PageIndex,request.PageSize, cancellationToken);

            if (request.PageSize < 1 && request.PageIndex < 1) { throw new Exception(); }
            if (request.PageSize < 1) { throw new Exception(); }
            if (request.PageIndex < 1) { throw new Exception(); }

            if (movies == null)
            {
                throw new Exception();
            }
            var moviesDto = _mapper.Map<List<Movie>, List<MoviesDto>>(movies.Items.ToList());

            var moviesVm = new MoviesVm()
            {
                CurrentPage = movies.CurrentPage,
                TotalItems = movies.TotalItems,
                TotalPages = movies.TotalPages,
                Items =  moviesDto
            };

            return moviesVm;
        }
    }
}
