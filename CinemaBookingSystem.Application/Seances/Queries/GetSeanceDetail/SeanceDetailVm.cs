using System;
using AutoMapper;
using CinemaBookingSystem.Application.Common.Mappings;
using CinemaBookingSystem.Domain.Entities;
using CinemaBookingSystem.Domain.Enums;

namespace CinemaBookingSystem.Application.Seances.Queries.GetSeanceDetail
{
    public class SeanceDetailVm : IMapFrom<Seance>
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public SeanceType SeanceType { get; set; }
        public CinemaHallDto CinemaHall { get; set; }
        public MovieDto Movie { get; set; }

        #region Mapping()
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Seance, SeanceDetailVm>()
                .ForMember(x => x.Id, map => map.MapFrom(src => src.Id))
                .ForMember(x => x.Date, map => map.MapFrom(src => src.Date))
                .ForMember(x => x.SeanceType, map => map.MapFrom(src => src.SeanceType));
        }
        #endregion
    }
}
