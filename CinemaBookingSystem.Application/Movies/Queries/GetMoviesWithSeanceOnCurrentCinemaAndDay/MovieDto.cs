using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CinemaBookingSystem.Application.Common.Mappings;
using CinemaBookingSystem.Domain.Entities;

namespace CinemaBookingSystem.Application.Movies.Queries.GetMoviesWithSeanceOnCurrentCinemaAndDay
{
    public class MovieDto : IMapFrom<Movie>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Plot { get; set; }
        public List<GenreDto> Genre { get; set; }
        public string Duration { get; set; }
        public string ImdbRating { get; set; }
        public string PosterPath { get; set; }
        public List<SeanceDto> Seances { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Movie, MovieDto>()
                .ForMember(d => d.Id, map => map.MapFrom(src => src.Id))
                .ForMember(d => d.Title, map => map.MapFrom(src => src.Title))
                .ForMember(d => d.Plot, map => map.MapFrom(src => src.Plot))
                .ForMember(d => d.PosterPath, map => map.MapFrom(src => src.PosterPath))
                .ForMember(d => d.Duration, map => map.MapFrom(src => src.Duration))
                .ForMember(d => d.ImdbRating, map => map.MapFrom(src => src.ImdbRating));
            profile.CreateMap<Seance, SeanceDto>()
                .ReverseMap();
        }
    }
}
