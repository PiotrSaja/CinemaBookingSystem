using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CinemaBookingSystem.Application.Common.Exceptions;
using CinemaBookingSystem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingSystem.Application.Seances.Queries.GetSeanceDetail
{
    public class GetSeanceDetailQueryHandler : IRequestHandler<GetSeanceDetailQuery, SeanceDetailVm>
    {
        private readonly ICinemaDbContext _context;
        private readonly IMapper _mapper;

        #region GetSeanceDetailQueryHandler()
        public GetSeanceDetailQueryHandler(ICinemaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #endregion

        #region Handle()
        public async Task<SeanceDetailVm> Handle(GetSeanceDetailQuery request, CancellationToken cancellationToken)
        {
            var seance = await _context.Seances
                .Include(x => x.Movie)
                .ThenInclude(x=>x.Genres)
                .Include(x => x.CinemaHall)
                .ThenInclude(x=>x.Cinema)
                .FirstOrDefaultAsync(x => x.Id == request.SeanceId && x.StatusId != 0, cancellationToken);

            if (seance == null)
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "Not exists in database or seanceId is incorrect");

            var seanceVm = _mapper.Map<SeanceDetailVm>(seance);

            return seanceVm;
        }
        #endregion
    }
}
