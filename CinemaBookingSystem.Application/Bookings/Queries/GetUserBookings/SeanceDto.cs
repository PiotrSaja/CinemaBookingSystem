using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CinemaBookingSystem.Application.Common.Mappings;
using CinemaBookingSystem.Domain.Entities;

namespace CinemaBookingSystem.Application.Bookings.Queries.GetUserBookings
{
    public class SeanceDto : IMapFrom<Seance>
    {
        public MovieDto Movie { get; set; }
        public CinemaHallDto CinemaHall { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Seance, SeanceDto>();
        }
    }
}
