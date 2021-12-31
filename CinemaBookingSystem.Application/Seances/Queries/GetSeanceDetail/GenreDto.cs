using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CinemaBookingSystem.Application.Common.Mappings;
using CinemaBookingSystem.Domain.Entities;

namespace CinemaBookingSystem.Application.Seances.Queries.GetSeanceDetail
{
    public class GenreDto : IMapFrom<Genre>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Genre, Movies.Queries.GetMovieDetail.GenreDto>()
                .ForMember(d => d.Id, map => map.MapFrom(src => src.Id))
                .ForMember(d => d.Name, map => map.MapFrom(src => src.Name))
                .ForMember(d => d.Description, map => map.MapFrom(src => src.Description));
        }
    }
}
