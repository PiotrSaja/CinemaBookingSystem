using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace CinemaBookingSystem.Application.Cinemas.Commands.DeleteCinema
{
    public class DeleteCinemaCommand : IRequest
    {
        public int CinemaId { get; set; }
    }
}
