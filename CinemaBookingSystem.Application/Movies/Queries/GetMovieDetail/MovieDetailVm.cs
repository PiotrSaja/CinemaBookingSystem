using System;
using System.Collections.Generic;
using AutoMapper;
using CinemaBookingSystem.Application.Common.Mappings;
using CinemaBookingSystem.Domain.Entities;

namespace CinemaBookingSystem.Application.Movies.Queries.GetMovieDetail
{
    public class MovieDetailVm : IMapFrom<Movie>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Plot { get; set; }
        public int Duration { get; set; }
        public ICollection<GenreDto> Genres { get; set; }
        public DirectorDto Director { get; set; }
        public ICollection<ActorDto> Actors { get; set; }
        public string Language { get; set; }
        public DateTime Released { get; set; }
        public string Country { get; set; }
        public string PosterPath { get; set; }
        public string ImdbRating { get; set; }
        public string BackgroundImagePath { get; set; }

        #region Mapping()
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Movie, MovieDetailVm>()
                .ForMember(d => d.Id, map => map.MapFrom(src => src.Id))
                .ForMember(d => d.Title, map => map.MapFrom(src => src.Title))
                .ForMember(d => d.Plot, map => map.MapFrom(src => src.Plot))
                .ForMember(d => d.Duration, map => map.MapFrom(src => src.Duration))
                .ForMember(d => d.Language, map => map.MapFrom(src => src.Language))
                .ForMember(d => d.Released, map => map.MapFrom(src => src.Released))
                .ForMember(d => d.Country, map => map.MapFrom(src => src.Country))
                .ForMember(d => d.PosterPath, map => map.MapFrom(src => src.PosterPath))
                .ForMember(d => d.ImdbRating, map => map.MapFrom(src => src.ImdbRating))
                .ForMember(d => d.BackgroundImagePath, map => map.MapFrom(src => src.BackgroundImagePath));
        }
        #endregion
    }
}
