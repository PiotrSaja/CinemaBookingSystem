using AutoMapper;
using CinemaBookingSystem.Application.Common.Mappings;
using CinemaBookingSystem.Domain.Entities;

namespace CinemaBookingSystem.Application.Bookings.Queries.GetUserBookings
{
    public class CinemaHallDto : IMapFrom<CinemaHall>
    {
        public CinemaDto Cinema { get; set; }

        #region Mapping()
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CinemaHall, CinemaHallDto>();
        }
        #endregion
    }
}
