using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CinemaBookingSystem.Application.Common.Interfaces;
using MediatR;

namespace CinemaBookingSystem.Application.Bookings.Commands.CreateBooking
{
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, int>
    {
        private readonly ICinemaDbContext _context;

        public CreateBookingCommandHandler(ICinemaDbContext context)
        {
            _context = context;
        }


        public Task<int> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
