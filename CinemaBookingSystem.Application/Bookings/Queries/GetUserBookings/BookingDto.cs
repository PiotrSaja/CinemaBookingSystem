﻿using System;
using AutoMapper;
using CinemaBookingSystem.Application.Common.Mappings;
using CinemaBookingSystem.Domain.Entities;
using CinemaBookingSystem.Domain.Enums;

namespace CinemaBookingSystem.Application.Bookings.Queries.GetUserBookings
{
    public class BookingDto : IMapFrom<Booking>
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public BookingStatus BookingStatus { get; set; }
        public SeanceDto Seance { get; set; }

        #region Mapping()
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Booking, BookingDto>()
                .ForMember(x => x.Id, map => map.MapFrom(src => src.Id))
                .ForMember(x => x.CreatedDate, map => map.MapFrom(src => src.Created))
                .ForMember(x => x.BookingStatus, map => map.MapFrom(src => src.BookingStatus));
        }
        #endregion
    }
}
