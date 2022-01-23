using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CinemaBookingSystem.Application.Common.Mappings;
using CinemaBookingSystem.Domain.Entities;
using CinemaBookingSystem.Domain.Enums;

namespace CinemaBookingSystem.Application.Bookings.Queries.GetUserBookingDetail
{
    public class BookingDetailVm : IMapFrom<Booking>
    {
        public int BookingId { get; set; }
        public int NumberOfSeats { get; set; }
        public BookingStatus BookingStatus { get; set; }
        public SeanceDto Seance { get; set; }
        public ICollection<SeanceSeatDto> SeanceSeats { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Booking, BookingDetailVm>()
                .ForMember(x => x.BookingId, map => map.MapFrom(src => src.Id))
                .ForMember(x => x.NumberOfSeats, map => map.MapFrom(src => src.NumberOfSeats))
                .ForMember(x => x.BookingStatus, map => map.MapFrom(src => src.BookingStatus));
        }
    }
}
