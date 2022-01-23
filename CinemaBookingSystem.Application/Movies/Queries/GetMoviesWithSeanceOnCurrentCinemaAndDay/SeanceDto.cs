using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CinemaBookingSystem.Application.Common.Mappings;
using CinemaBookingSystem.Domain.Entities;
using CinemaBookingSystem.Domain.Enums;

namespace CinemaBookingSystem.Application.Movies.Queries.GetMoviesWithSeanceOnCurrentCinemaAndDay
{
    public class SeanceDto :IMapFrom<Seance>
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public SeanceType SeanceType { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Seance, SeanceDto>()
                .ForMember(x => x.Id, map => map.MapFrom(src => src.Id))
                .ForMember(x => x.Date, map => map.MapFrom(src => src.Date))
                .ForMember(x => x.SeanceType, map => map.MapFrom(src => src.SeanceType));
        }
    }
}
