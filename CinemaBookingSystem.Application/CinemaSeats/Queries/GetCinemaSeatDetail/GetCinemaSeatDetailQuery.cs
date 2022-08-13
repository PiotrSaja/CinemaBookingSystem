using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace CinemaBookingSystem.Application.CinemaSeats.Queries.GetCinemaSeatDetail
{
    public class GetCinemaSeatDetailQuery : IRequest<CinemaSeatDetailVm>
    {
        public int CinemaSeatId { get; set; }
    }
}
