using System.Threading;
using System.Threading.Tasks;
using CinemaBookingSystem.Application.Common.Interfaces;
using CinemaBookingSystem.Domain.Entities;
using CinemaBookingSystem.Domain.ValueObjects;
using MediatR;

namespace CinemaBookingSystem.Application.Cinemas.Commands.CreateCinema
{
    public class CreateCinemaCommandHandler : IRequestHandler<CreateCinemaCommand,int>
    {
        private readonly ICinemaDbContext _context;

        #region CreateCinemaCommandHandler()
        public CreateCinemaCommandHandler(ICinemaDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Handle()
        public async Task<int> Handle(CreateCinemaCommand request, CancellationToken cancellationToken)
        {
            var cinema = new Cinema()
            {
                Name = request.Name,
                TotalCinemaHalls = request.TotalCinemaHalls,
                Address = new Address()
                {
                    City = request.City,
                    Country = request.Country,
                    State = request.State,
                    Street = request.Street,
                    ZipCode = request.ZipCode
                },
                ImagePath = request.ImagePath
            };

            _context.Cinemas.Add(cinema);

            await _context.SaveChangesAsync(cancellationToken);

            return cinema.Id;
        }
        #endregion
    }
}
