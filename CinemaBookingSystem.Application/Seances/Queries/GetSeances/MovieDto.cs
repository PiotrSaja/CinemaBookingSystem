using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CinemaBookingSystem.Application.Common.Mappings;
using CinemaBookingSystem.Domain.Entities;

namespace CinemaBookingSystem.Application.Seances.Queries.GetSeances
{
    public class MovieDto : IMapFrom<Movie>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Plot { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Movie, MovieDto>()
                .ForMember(d => d.Id, map => map.MapFrom(src => src.Id))
                .ForMember(d => d.Title, map => map.MapFrom(src => src.Title))
                .ForMember(d => d.Plot, map => map.MapFrom(src => src.Plot))
                .ForMember(d => d.Language, map => map.MapFrom(src => src.Language))
                .ForMember(d => d.Country, map => map.MapFrom(src => src.Country));
        }
    }
}