using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBookingSystem.Application.Common.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    namespace TicketBookingSystem.Application.Common.Interfaces
    {
        public interface ITmdbClient
        {
            Task<string> GetMovieByImdbId(string id, CancellationToken cancellationToken);
        }
    }
}
