using AutoMapper;
using CinemaBookingSystem.Application.Common.Mappings;
using CinemaBookingSystem.Domain.Entities;

namespace CinemaBookingSystem.Application.Bookings.Queries.GetUserBookings
{
    public class SeanceDto : IMapFrom<Seance>
    {
        public MovieDto Movie { get; set; }
        public CinemaHallDto CinemaHall { get; set; }

        #region Mapping()
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Seance, SeanceDto>();
        }
        #endregion
    }
}
