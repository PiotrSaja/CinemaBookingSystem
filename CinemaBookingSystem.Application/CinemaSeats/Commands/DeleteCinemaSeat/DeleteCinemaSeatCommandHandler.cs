﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CinemaBookingSystem.Application.Common.Exceptions;
using CinemaBookingSystem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingSystem.Application.CinemaSeats.Commands.DeleteCinemaSeat
{
    public class DeleteCinemaSeatCommandHandler : IRequestHandler<DeleteCinemaSeatCommand, Unit>
    {
        private readonly ICinemaDbContext _context;

        public DeleteCinemaSeatCommandHandler(ICinemaDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteCinemaSeatCommand request, CancellationToken cancellationToken)
        {
            var cinemaSeatToDelete = await _context.CinemaSeats.Where(x => x.Id == request.CinemaSeatId && x.StatusId != 0)
                .FirstOrDefaultAsync(cancellationToken);

            if (cinemaSeatToDelete == null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "Not exists in database, check your id");
            }

            _context.CinemaSeats.Remove(cinemaSeatToDelete);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
