using System.Collections.Generic;
using AutoMapper;
using CinemaBookingSystem.Application.Common.Mappings;
using CinemaBookingSystem.Domain.Entities;
using CinemaBookingSystem.Domain.Enums;
using CinemaBookingSystem.Domain.ValueObjects;

namespace CinemaBookingSystem.Application.Bookings.Queries.GetBookingDetail
{
    public class BookingDetailVm : IMapFrom<Booking>
    {
        public int Id { get; set; }
        public PersonalName PersonalName { get; set; }
        public int NumberOfSeats { get; set; }
        public BookingStatus Status { get; set; }
        public string UserId { get; set; }
        public SeanceDto Seance { get; set; }
        public ICollection<SeanceSeatDto> SeanceSeats { get; set; }

        #region Mapping()
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Booking, BookingDetailVm>()
                .ForMember(x => x.Id, map => map.MapFrom(src => src.Id))
                .ForMember(x => x.NumberOfSeats, map => map.MapFrom(src => src.NumberOfSeats))
                .ForMember(x => x.Status, map => map.MapFrom(src => src.BookingStatus))
                .ForMember(x => x.UserId, map => map.MapFrom(src => src.UserId))
                .ForMember(x => x.PersonalName, map => map.MapFrom(src => src.PersonalName));
        }
        #endregion
    }
}
