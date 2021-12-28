using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace CinemaBookingSystem.Application.Cinemas.Queries.GetCinemaDetail
{
    public class GetCinemaDetailQuery : IRequest<CinemaDetailVm>
    {
        public int CinemaId { get; set; }
    }
}
