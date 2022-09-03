using System.Collections.Generic;
using AutoMapper;
using CinemaBookingSystem.Application.Common.Mappings;
using CinemaBookingSystem.Domain.Entities;

namespace CinemaBookingSystem.Application.Movies.Queries.GetMovies
{
    public class MoviesDto : IMapFrom<Movie>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Plot { get; set; }
        public ICollection<GenreDto> Genres { get; set; }
        public string PosterPath { get; set; }

        #region Mapping()
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Movie, MoviesDto>()
                .ForMember(d => d.Id, map => map.MapFrom(src => src.Id))
                .ForMember(d => d.Title, map => map.MapFrom(src => src.Title))
                .ForMember(d => d.Plot, map => map.MapFrom(src => src.Plot))
                .ForMember(d => d.PosterPath, map => map.MapFrom(src => src.PosterPath));
        }
        #endregion
    }
}
