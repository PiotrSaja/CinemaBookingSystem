using System.Collections.Generic;

namespace CinemaBookingSystem.Application.Seances.Queries.GetSeances
{
    public class SeancesVm
    {
        public int CurrentPage { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public string SearchString { get; set; }
        public ICollection<SeanceDto> Items { get; set; }
    }
}
