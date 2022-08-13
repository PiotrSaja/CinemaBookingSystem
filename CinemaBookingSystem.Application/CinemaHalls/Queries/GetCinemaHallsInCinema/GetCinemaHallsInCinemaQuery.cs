using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace CinemaBookingSystem.Application.CinemaHalls.Queries.GetCinemaHallsInCinema
{
    public class GetCinemaHallsInCinemaQuery : IRequest<CinemaHallsVm>
    {
        public int CinemaId { get; set; }
    }
}
