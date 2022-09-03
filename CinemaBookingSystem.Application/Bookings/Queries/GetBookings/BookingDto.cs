using System;
using AutoMapper;
using CinemaBookingSystem.Application.Common.Mappings;
using CinemaBookingSystem.Domain.Entities;
using CinemaBookingSystem.Domain.Enums;
using CinemaBookingSystem.Domain.ValueObjects;

namespace CinemaBookingSystem.Application.Bookings.Queries.GetBookings
{
    public class BookingDto : IMapFrom<Booking>
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public PersonalName PersonalName { get; set; }
        public string UserId { get; set; }
        public BookingStatus BookingStatus { get; set; }

        #region Mapping()
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Booking, BookingDto>()
                .ForMember(x => x.Id, map => map.MapFrom(src => src.Id))
                .ForMember(x => x.CreatedDate, map => map.MapFrom(src => src.Created))
                .ForMember(x => x.PersonalName, map => map.MapFrom(src => src.PersonalName))
                .ForMember(x => x.UserId, map => map.MapFrom(src => src.UserId))
                .ForMember(x => x.BookingStatus, map => map.MapFrom(src => src.BookingStatus));
        }
        #endregion
    }
}
