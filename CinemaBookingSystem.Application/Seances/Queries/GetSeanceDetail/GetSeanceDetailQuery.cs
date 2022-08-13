using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace CinemaBookingSystem.Application.Seances.Queries.GetSeanceDetail
{
    public class GetSeanceDetailQuery : IRequest<SeanceDetailVm>
    {
        public int SeanceId { get; set; }
    }
}
