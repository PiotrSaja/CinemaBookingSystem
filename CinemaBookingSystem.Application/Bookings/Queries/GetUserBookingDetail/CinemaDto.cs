﻿using AutoMapper;
using CinemaBookingSystem.Application.Common.Mappings;
using CinemaBookingSystem.Domain.Entities;

namespace CinemaBookingSystem.Application.Bookings.Queries.GetUserBookingDetail
{
    public class CinemaDto : IMapFrom<Cinema>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string City { get; set; }

        #region Mapping()
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Cinema, CinemaDto>()
                .ForMember(x => x.Id, map => map.MapFrom(src => src.Id))
                .ForMember(x => x.Name, map => map.MapFrom(src => src.Name))
                .ForMember(x => x.Street, map => map.MapFrom(src => src.Address.Street))
                .ForMember(x => x.City, map => map.MapFrom(src => src.Address.City));
        }
        #endregion
    }
}
