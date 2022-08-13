using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace CinemaBookingSystem.Application.CinemaHalls.Queries.GetCinemaHallDetail
{
    public class GetCinemaHallDetailQuery : IRequest<CinemaHallDetailVm>
    {
        public int CinemaHallId { get; set; }
    }
}
