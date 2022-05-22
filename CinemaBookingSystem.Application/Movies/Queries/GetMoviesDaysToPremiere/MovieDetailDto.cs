using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CinemaBookingSystem.Application.Common.Mappings;
using CinemaBookingSystem.Application.Movies.Queries.GetMovieDetail;
using CinemaBookingSystem.Domain.Entities;

namespace CinemaBookingSystem.Application.Movies.Queries.GetMoviesDaysToPremiere
{
    public class MovieDetailDto : IMapFrom<Movie>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Plot { get; set; }
        public ICollection<GenreDto> Genre { get; set; }
        public string PosterPath { get; set; }

        public DateTime ReleasedDate { get; set; }
        public string ImdbRating { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Movie, MovieDetailDto>()
                .ForMember(d => d.Id, map => map.MapFrom(src => src.Id))
                .ForMember(d => d.Title, map => map.MapFrom(src => src.Title))
                .ForMember(d => d.Plot, map => map.MapFrom(src => src.Plot))
                .ForMember(d => d.PosterPath, map => map.MapFrom(src => src.PosterPath))
                .ForMember(d => d.ReleasedDate, map => map.MapFrom(src => src.Released))
                .ForMember(d => d.ImdbRating, map => map.MapFrom(src => src.ImdbRating));
        }
    }
}
