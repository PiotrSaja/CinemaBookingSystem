using AutoMapper;
using CinemaBookingSystem.Application.Common.Mappings;
using CinemaBookingSystem.Domain.Entities;

namespace CinemaBookingSystem.Application.Movies.Queries.GetMovieDetail
{
    public class DirectorDto : IMapFrom<Director>
    {
        public int Id { get; set; }
        public string FullName { get; set; }

        #region Mapping()
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Director, DirectorDto>()
                .ForMember(x => x.Id, map => map.MapFrom(src => src.Id))
                .ForMember(x => x.FullName, map => map.MapFrom(src => src.DirectorName.ToString()));
        }
        #endregion
    }
}
