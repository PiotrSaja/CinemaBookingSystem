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
    public class MovieDto : IMapFrom<Movie>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Plot { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }
        public ICollection<GenreMDto> Genres { get; set; }
        public string ImdbRating { get; set; }
        public string PosterPath { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Movie, MovieDto>()
                .ForMember(d => d.Id, map => map.MapFrom(src => src.Id))
                .ForMember(d => d.Title, map => map.MapFrom(src => src.Title))
                .ForMember(d => d.Plot, map => map.MapFrom(src => src.Plot))
                .ForMember(d => d.Language, map => map.MapFrom(src => src.Language))
                .ForMember(d => d.Country, map => map.MapFrom(src => src.Country))
                .ForMember(d => d.PosterPath, map => map.MapFrom(src => src.PosterPath))
                .ForMember(d => d.ImdbRating, map => map.MapFrom(src => src.ImdbRating));
        }
    }
}
