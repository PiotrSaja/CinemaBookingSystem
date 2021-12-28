using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CinemaBookingSystem.Application.Common.Mappings;
using CinemaBookingSystem.Domain.Entities;

namespace CinemaBookingSystem.Application.Cinemas.Queries.GetCinemaDetail
{
    public class CinemaDetailVm : IMapFrom<Cinema>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TotalCinemaHalls { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string ImagePath { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Cinema, CinemaDetailVm>()
                .ForMember(x => x.Id, map => map.MapFrom(src => src.Id))
                .ForMember(x => x.Name, map => map.MapFrom(src => src.Name))
                .ForMember(x => x.TotalCinemaHalls, map => map.MapFrom(src => src.TotalCinemaHalls))
                .ForMember(x => x.Street, map => map.MapFrom(src => src.Address.Street))
                .ForMember(x => x.City, map => map.MapFrom(src => src.Address.City))
                .ForMember(x => x.State, map => map.MapFrom(src => src.Address.State))
                .ForMember(x => x.Country, map => map.MapFrom(src => src.Address.Country))
                .ForMember(x => x.ZipCode, map => map.MapFrom(src => src.Address.ZipCode))
                .ForMember(x => x.ImagePath, map => map.MapFrom(src => src.ImagePath));
        }
    }
}
