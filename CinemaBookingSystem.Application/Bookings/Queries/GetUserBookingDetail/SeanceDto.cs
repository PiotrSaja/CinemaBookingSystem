﻿using System;
using AutoMapper;
using CinemaBookingSystem.Application.Common.Mappings;
using CinemaBookingSystem.Domain.Entities;
using CinemaBookingSystem.Domain.Enums;

namespace CinemaBookingSystem.Application.Bookings.Queries.GetUserBookingDetail
{
    public class SeanceDto : IMapFrom<Seance>
    {
        public DateTime Date { get; set; }
        public SeanceType SeanceType { get; set; }
        public MovieDto Movie { get; set; }
        public CinemaHallDto CinemaHall { get; set; }

        #region Mapping()
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Seance, SeanceDto>()
                .ForMember(x => x.Date, map => map.MapFrom(src => src.Date))
                .ForMember(x => x.SeanceType, map => map.MapFrom(src => src.SeanceType));
        }
        #endregion
    }
}
