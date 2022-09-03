using AutoMapper;
using CinemaBookingSystem.Application.Common.Mappings;
using CinemaBookingSystem.Domain.Entities;

namespace CinemaBookingSystem.Application.Seances.Queries.GetSeanceDetail
{
    public class CinemaHallDto : IMapFrom<CinemaHall>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NumberOfColumns { get; set; }
        public int NumberOfRows { get; set; }
        public CinemaSDto Cinema { get; set; }

        #region Mapping()
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CinemaHall, CinemaHallDto>()
                .ForMember(x => x.Id, map => map.MapFrom(src => src.Id))
                .ForMember(x => x.Name, map => map.MapFrom(src => src.Name))
                .ForMember(x => x.NumberOfColumns, map => map.MapFrom(src => src.NumberOfColumns))
                .ForMember(x => x.NumberOfRows, map => map.MapFrom(src => src.NumberOfRows));
        }
        #endregion
    }
}
