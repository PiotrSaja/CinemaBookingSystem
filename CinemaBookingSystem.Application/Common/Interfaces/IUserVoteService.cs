using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CinemaBookingSystem.Application.Common.Models;
using Hangfire.Server;

namespace CinemaBookingSystem.Application.Common.Interfaces
{
    public interface IUserVoteService
    {
        public Task<bool> Clustering(PerformContext context, CancellationToken cancellationToken);
        public Task<List<MovieResultAssign>> GetPredictions(string currentUserId, CancellationToken cancellationToken);
    }
}
