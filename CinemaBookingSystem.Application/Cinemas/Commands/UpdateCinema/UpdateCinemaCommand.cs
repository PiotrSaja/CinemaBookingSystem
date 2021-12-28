using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace CinemaBookingSystem.Application.Cinemas.Commands.UpdateCinema
{
    public class UpdateCinemaCommand : IRequest<int>
    {
        public int CinemaId { get; set; }
        public string Name { get; set; }
        public int TotalCinemaHalls { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string ImagePath { get; set; }
    }
}
