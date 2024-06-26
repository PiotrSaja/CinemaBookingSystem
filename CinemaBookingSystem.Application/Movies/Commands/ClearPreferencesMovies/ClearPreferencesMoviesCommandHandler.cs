﻿using CinemaBookingSystem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CinemaBookingSystem.Domain.Enums;

namespace CinemaBookingSystem.Application.Movies.Commands.ClearPreferencesMovies
{
    public class ClearPreferencesMoviesCommandHandler : IRequestHandler<ClearPreferencesMoviesCommand, bool>
    {
        private readonly ICinemaDbContext _context;
        private readonly IUserService _userService;

        #region ClearPreferencesMoviesCommandHandler()
        public ClearPreferencesMoviesCommandHandler(ICinemaDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }
        #endregion

        #region Handle()
        public async Task<bool> Handle(ClearPreferencesMoviesCommand request, CancellationToken cancellationToken)
        {
            var userMovie = await _context.UserPreferencesMovies.Where(x => x.UserId == _userService.Id && x.StatusId != 0).ToListAsync(cancellationToken);

            foreach (var movie in userMovie)
                _context.UserPreferencesMovies.Remove(movie);

            var userRecommendation = await _context.UserRecommendationTypes.FirstOrDefaultAsync(x => x.UserId == _userService.Id,
                cancellationToken);

            userRecommendation.RecommendationType = RecommendationType.None;

            _context.UserRecommendationTypes.Update(userRecommendation);

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
        #endregion
    }
}
