using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CinemaBookingSystem.Application.Common.Interfaces;
using CinemaBookingSystem.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaBookingSystem.Api.Services
{
    public class SeatLockingService : ISeatLockingService
    {
        private readonly IServiceScopeFactory scopeFactory;
        public List<SeatLockingModel> LockedList { get; set; }

        public SeatLockingService(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
            LockedList = new List<SeatLockingModel>();
        }

        public async Task<bool> LockSeat(int seanceSeatId, string userId, DateTime expirationTime)
        {
            var lockedSeat = LockedList.FirstOrDefault(x => x.SeanceSeatId == seanceSeatId);
            if (lockedSeat == null)
            {
                using (var scope = scopeFactory.CreateScope())
                {
                    var lockSeat = new SeatLockingModel()
                    {
                        SeanceSeatId = seanceSeatId,
                        UserId = userId,
                        ExpirationTime = expirationTime
                    };
                    LockedList.Add(lockSeat);

                    var _context = scope.ServiceProvider.GetRequiredService<ICinemaDbContext>();
                    var seatToUpdate = await _context.SeanceSeats.FirstOrDefaultAsync(x => x.Id == lockSeat.SeanceSeatId);
                    seatToUpdate.SeatStatus = true;
                    _context.SeanceSeats.Update(seatToUpdate);
                    await _context.SaveChangesAsync(CancellationToken.None);
                }
                return true;
            }
            if (lockedSeat.UserId.Equals(userId))
            {
                return true;
            }
            if (!lockedSeat.UserId.Equals(userId))
            {
                return false;
            }
            if (LockedList.Count(x => x.UserId == userId) > 5)
            {
                return false;
            }

            return false;
        }
    }
}
