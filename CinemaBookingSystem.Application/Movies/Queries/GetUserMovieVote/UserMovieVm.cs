using AutoMapper;
using CinemaBookingSystem.Application.Common.Mappings;
using CinemaBookingSystem.Domain.Entities;

namespace CinemaBookingSystem.Application.Movies.Queries.GetUserMovieVote
{
    public class UserMovieVm : IMapFrom<UserMovieVote>
    {
        public double Rating { get; set; }

        #region Mapping()
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserMovieVote, UserMovieVm>()
                .ForMember(d => d.Rating, map => map.MapFrom(src => src.Vote));
        }
        #endregion
    }
}
