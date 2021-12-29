using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CinemaBookingSystem.Application.Common.Mappings;
using CinemaBookingSystem.Domain.Entities;

namespace CinemaBookingSystem.Application.CinemaHalls.Queries.GetCinemaHallsInCinema
{
    public class CinemaHallDto : IMapFrom<CinemaHall>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TotalSeats { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CinemaHall, CinemaHallDto>()
                .ForMember(x => x.Id, map => map.MapFrom(src => src.Id))
                .ForMember(x => x.Name, map => map.MapFrom(src => src.Name))
                .ForMember(x => x.TotalSeats, map => map.MapFrom(src => src.TotalSeats));
        }
    }
}
