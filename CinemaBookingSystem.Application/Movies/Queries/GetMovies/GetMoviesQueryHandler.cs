﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CinemaBookingSystem.Application.Common.Exceptions;
using CinemaBookingSystem.Application.Common.Extensions;
using CinemaBookingSystem.Application.Common.Interfaces;
using CinemaBookingSystem.Application.Common.Models;
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
            PagedModel<Movie> movies;

            if (request.PageSize < 1 && request.PageIndex < 1) { throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, "Page size and Page index can't be null or less than 1"); }
            if (request.PageSize < 1) { throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, "Page size can't be null or less than 1"); }
            if (request.PageIndex < 1) { throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, "Page index can't be null or less than 1"); }

            if (!String.IsNullOrEmpty(request.SearchString))
            {
                movies = await _context.Movies
                    .Where(x => x.StatusId != 0 &&
                                x.Title.Contains(request.SearchString))
                    .AsNoTracking()
                    .OrderBy(p => p.Created)
                    .PaginateAsync(request.PageIndex, request.PageSize, cancellationToken);
            }
            else
            {
                movies = await _context.Movies
                    .Where(x => x.StatusId != 0)
                    .Include(x => x.Genres)
                    .AsNoTracking()
                    .PaginateAsync(request.PageIndex, request.PageSize, cancellationToken);
            }

            if (movies == null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "Not exists records in database");
            }
            var moviesDto = _mapper.Map<List<Movie>, List<MoviesDto>>(movies.Items.ToList());

            var moviesVm = new MoviesVm()
            {
                CurrentPage = movies.CurrentPage,
                TotalItems = movies.TotalItems,
                TotalPages = movies.TotalPages,
                SearchString = request.SearchString,
                Items =  moviesDto
            };

            return moviesVm;
        }
    }
}
