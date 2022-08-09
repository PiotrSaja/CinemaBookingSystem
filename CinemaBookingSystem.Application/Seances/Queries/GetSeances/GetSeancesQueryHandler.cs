using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CinemaBookingSystem.Application.Common.Exceptions;
using CinemaBookingSystem.Application.Common.Extensions;
using CinemaBookingSystem.Application.Common.Interfaces;
using CinemaBookingSystem.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingSystem.Application.Seances.Queries.GetSeances
{
    public class GetSeancesQueryHandler : IRequestHandler<GetSeancesQuery, SeancesVm>
    {
        private readonly ICinemaDbContext _context;
        private readonly IMapper _mapper;

        public GetSeancesQueryHandler(ICinemaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<SeancesVm> Handle(GetSeancesQuery request, CancellationToken cancellationToken)
        {
            if (request.PageSize < 1 && request.PageIndex < 1) { throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, "Page size and Page index can't be null or less than 1"); }
            if (request.PageSize < 1) { throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, "Page size can't be null or less than 1"); }
            if (request.PageIndex < 1) { throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, "Page index can't be null or less than 1"); }

            var seances = await _context.Seances.Where(x => x.StatusId != 0)
                .AsNoTracking()
                .OrderBy(p => p.Created)
                .Include(x => x.Movie)
                .Include(x => x.CinemaHall)
                .Include(x=>x.SeanceSeats)
                .PaginateAsync(request.PageIndex, request.PageSize, cancellationToken);

            if (!String.IsNullOrEmpty(request.SearchString))
            {
                seances = await _context.Seances
                    .Where(x => x.StatusId != 0
                                && x.Movie.Title.Contains(request.SearchString))
                    .OrderBy(p => p.Created)
                    .Include(x => x.Movie)
                    .Include(x => x.CinemaHall)
                    .Include(x=>x.SeanceSeats)
                    .AsNoTracking()
                    .PaginateAsync(request.PageIndex, request.PageSize, cancellationToken);
            }

            var showsDto = _mapper.Map<List<Seance>, List<SeanceDto>>(seances.Items.ToList());

            if (seances == null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "Not exists records in database");
            }

            var seancesVm = new SeancesVm()
            {
                CurrentPage = seances.CurrentPage,
                TotalItems = seances.TotalItems,
                TotalPages = seances.TotalPages,
                SearchString = request.SearchString,
                Items = showsDto
            };

            return seancesVm;
        }
    }
}
