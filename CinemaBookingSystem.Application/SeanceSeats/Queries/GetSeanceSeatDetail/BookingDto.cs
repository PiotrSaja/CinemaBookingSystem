using AutoMapper;
using CinemaBookingSystem.Application.Common.Mappings;
using CinemaBookingSystem.Domain.Entities;
using CinemaBookingSystem.Domain.ValueObjects;

namespace CinemaBookingSystem.Application.SeanceSeats.Queries.GetSeanceSeatDetail
{
    public class BookingDto : IMapFrom<Booking>
    {
        public int Id { get; set; }
        public PersonalName PersonalName { get; set; }
        public string UserId { get; set; }

        #region Mapping()
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Booking, BookingDto>()
                .ForMember(x=>x.Id, map => map.MapFrom(src => src.Id))
                .ForMember(x=>x.UserId, map => map.MapFrom(src => src.UserId))
                .ForMember(x=>x.PersonalName, map => map.MapFrom(src => src.PersonalName));
        }
        #endregion
    }
}
