using System;
using CinemaBookingSystem.Application.Common.Interfaces;

namespace CinemaBookingSystem.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        #region Now()
        public DateTime Now => DateTime.Now;
        #endregion
    }
}
