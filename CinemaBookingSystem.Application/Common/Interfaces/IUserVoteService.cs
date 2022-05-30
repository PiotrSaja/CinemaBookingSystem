using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CinemaBookingSystem.Application.Common.Interfaces
{
    public interface IUserVoteService
    {
        public Task<bool> Clustering(CancellationToken cancellationToken);
        public Task<List<int>> GetPredictions(CancellationToken cancellationToken);
    }
}
