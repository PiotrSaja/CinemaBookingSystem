using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CinemaBookingSystem.Application.Common.Mappings;
using CinemaBookingSystem.Domain.Entities;

namespace CinemaBookingSystem.Application.Bookings.Queries.GetUserBookingDetail
{
    public class MovieDto : IMapFrom<Movie>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string PosterPath { get; set; }
        public string BackgroundImagePath { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Movie, MovieDto>()
                .ForMember(d => d.Id, map => map.MapFrom(src => src.Id))
                .ForMember(d => d.Title, map => map.MapFrom(src => src.Title))
                .ForMember(d => d.PosterPath, map => map.MapFrom(src => src.PosterPath))
                .ForMember(d => d.BackgroundImagePath, map => map.MapFrom(src => src.BackgroundImagePath));
        }
    }
}
