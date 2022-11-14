﻿using CinemaBookingSystem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
            var userMovie = await _context.UserPreferencesMovies.Where(x => x.UserId == _userService.Id).ToListAsync(cancellationToken);

            if (userMovie.Count == 0)
                return false;
            
            _context.UserPreferencesMovies.RemoveRange(userMovie);

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
        #endregion
    }
}
