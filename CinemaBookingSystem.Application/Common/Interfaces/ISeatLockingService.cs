using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaBookingSystem.Application.Common.Models;

namespace CinemaBookingSystem.Application.Common.Interfaces
{
    public interface ISeatLockingService
    {
        List<SeatLockingModel> LockedList { get; set; }

        Task<bool> LockSeat(int showSeatId, string userId, DateTime expirationTime);
    }
}
