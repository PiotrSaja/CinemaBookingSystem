using System.Threading;
using System.Threading.Tasks;

namespace CinemaBookingSystem.Application.Common.Interfaces
{
    public interface ITmdbClient
    {
        Task<string> GetMovieByImdbId(string id, CancellationToken cancellationToken);
    }
}
