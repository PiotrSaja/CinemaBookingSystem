using AutoMapper;
using CinemaBookingSystem.Application.Common.Mappings;
using CinemaBookingSystem.Domain.Entities;

namespace CinemaBookingSystem.Application.Seances.Queries.GetSeanceDetail
{
    public class GenreMDto : IMapFrom<Genre>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        #region Mapping()
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Genre, GenreMDto>()
                .ForMember(d => d.Id, map => map.MapFrom(src => src.Id))
                .ForMember(d => d.Name, map => map.MapFrom(src => src.Name));
        }
        #endregion
    }
}
