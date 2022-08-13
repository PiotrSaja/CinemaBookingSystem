using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CinemaBookingSystem.Application.Common.Interfaces
{
    public interface IOmdbClient
    {
        Task<string> GetMovie(string searchFilter, CancellationToken cancellationToken);
        Task<string> GetMovieById(string id, CancellationToken cancellationToken);
    }
}
