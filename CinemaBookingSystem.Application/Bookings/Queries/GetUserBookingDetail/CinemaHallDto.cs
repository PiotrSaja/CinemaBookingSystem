using AutoMapper;
using CinemaBookingSystem.Application.Common.Mappings;
using CinemaBookingSystem.Domain.Entities;

namespace CinemaBookingSystem.Application.Bookings.Queries.GetUserBookingDetail
{
    public class CinemaHallDto : IMapFrom<CinemaHall>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CinemaDto Cinema { get; set; }

        #region Mapping()
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CinemaHall, CinemaHallDto>()
                .ForMember(x => x.Id, map => map.MapFrom(src => src.Id))
                .ForMember(x => x.Name, map => map.MapFrom(src => src.Name));
        }
        #endregion
    }
}
