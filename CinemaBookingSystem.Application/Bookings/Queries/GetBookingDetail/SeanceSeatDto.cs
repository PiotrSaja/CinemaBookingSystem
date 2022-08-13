using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CinemaBookingSystem.Application.Common.Mappings;
using CinemaBookingSystem.Domain.Entities;

namespace CinemaBookingSystem.Application.Bookings.Queries.GetBookingDetail
{
    public class SeanceSeatDto : IMapFrom<SeanceSeat>
    {
        public int Id { get; set; }
        public bool SeatStatus { get; set; }
        public double Price { get; set; }
        public int BookingId { get; set; }
        public int SeanceId { get; set; }
        public CinemaSeatDto CinemaSeat { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<SeanceSeat, SeanceSeatDto>()
                .ForMember(x => x.Id, map => map.MapFrom(src => src.Id))
                .ForMember(x => x.SeatStatus, map => map.MapFrom(src => src.SeatStatus))
                .ForMember(x => x.Price, map => map.MapFrom(src => src.Price))
                .ForMember(x => x.BookingId, map => map.MapFrom(src => src.BookingId))
                .ForMember(x => x.SeanceId, map => map.MapFrom(src => src.SeanceId));
        }
    }
}
