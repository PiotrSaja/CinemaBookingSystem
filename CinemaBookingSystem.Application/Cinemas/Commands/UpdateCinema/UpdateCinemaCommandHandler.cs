using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CinemaBookingSystem.Application.Common.Exceptions;
using CinemaBookingSystem.Application.Common.Interfaces;
using CinemaBookingSystem.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingSystem.Application.Cinemas.Commands.UpdateCinema
{
    public class UpdateCinemaCommandHandler : IRequestHandler<UpdateCinemaCommand, int>
    {
        private readonly ICinemaDbContext _context;

        #region UpdateCinemaCommandHandler()
        public UpdateCinemaCommandHandler(ICinemaDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Handle()
        public async Task<int> Handle(UpdateCinemaCommand request, CancellationToken cancellationToken)
        {
            var cinemaToUpdate = await _context.Cinemas
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (cinemaToUpdate == null)
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "Not exists in database, check your id");
            
            cinemaToUpdate.Name = request.Name;
            cinemaToUpdate.TotalCinemaHalls = request.TotalCinemaHalls;
            cinemaToUpdate.Address = new Address()
            {
                City = request.City,
                Country = request.Country,
                State = request.State,
                Street = request.Street,
                ZipCode = request.ZipCode
            };
            cinemaToUpdate.ImagePath = request.ImagePath;

            await _context.SaveChangesAsync(cancellationToken);

            return cinemaToUpdate.Id;
        }
        #endregion
    }
}
