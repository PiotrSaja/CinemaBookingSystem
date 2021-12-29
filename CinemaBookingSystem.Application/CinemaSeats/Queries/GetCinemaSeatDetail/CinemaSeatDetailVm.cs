using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CinemaBookingSystem.Application.Common.Mappings;
using CinemaBookingSystem.Domain.Entities;
using CinemaBookingSystem.Domain.Enums;

namespace CinemaBookingSystem.Application.CinemaSeats.Queries.GetCinemaSeatDetail
{
    public class CinemaSeatDetailVm : IMapFrom<CinemaSeat>
    {
        public int Id { get; set; }
        public int SeatNumber { get; set; }
        public int Row { get; set; }
        public SeatType SeatType { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CinemaSeat, CinemaSeatDetailVm>()
                .ForMember(x => x.Id, map => map.MapFrom(src => src.Id))
                .ForMember(x => x.SeatNumber, map => map.MapFrom(src => src.SeatNumber))
                .ForMember(x => x.Row, map => map.MapFrom(src => src.Row))
                .ForMember(x => x.SeatType, map => map.MapFrom(src => src.SeatType));
        }
    }
}
