using AutoMapper;
using CinemaBookingSystem.Application.Common.Mappings;
using CinemaBookingSystem.Domain.Entities;

namespace CinemaBookingSystem.Application.SeanceSeats.Queries.GetSeanceSeatDetail
{
    public class SeanceSeatVm : IMapFrom<SeanceSeat>
    {
        public int Id { get; set; }
        public bool SeatStatus { get; set; }
        public double Price { get; set; }
        public CinemaSeatDto CinemaSeat { get; set; }
        public BookingDto Booking { get; set; }
        public SeanceDto Seance { get; set; }

        #region Mapping()
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SeanceSeat, SeanceSeatVm>()
                .ForMember(x => x.Id, map => map.MapFrom(src => src.Id))
                .ForMember(x => x.SeatStatus, map => map.MapFrom(src => src.SeatStatus))
                .ForMember(x => x.Price, map => map.MapFrom(src => src.Price));
        }
        #endregion
    }
}
