using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CinemaBookingSystem.Application.Common.Models;

namespace CinemaBookingSystem.Application.Common.Interfaces
{
    public interface IUserVoteService
    {
        public Task<bool> Clustering(CancellationToken cancellationToken);
        public Task<List<MovieResultAssign>> GetPredictions(string currentUserId, CancellationToken cancellationToken);
    }
}
