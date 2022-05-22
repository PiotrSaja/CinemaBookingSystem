using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CinemaBookingSystem.Application.Common.Interfaces;
using CinemaBookingSystem.Application.Common.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CinemaBookingSystem.Api.Services
{
    public class SeatLockingHostedService : IHostedService
    {
        private readonly ISeatLockingService _seatLockingService;
        private readonly IDateTime _dateTime;
        private readonly ILogger<SeatLockingHostedService> _logger;
        private readonly IServiceScopeFactory scopeFactory;

        public SeatLockingHostedService(ISeatLockingService seatLockingService, IDateTime dateTime, ILogger<SeatLockingHostedService> logger, IServiceScopeFactory factory)
        {
            _seatLockingService = seatLockingService;
            _dateTime = dateTime;
            _logger = logger;
            scopeFactory = factory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        _logger.LogInformation($"CinemaBookingSystem - SeatLockingHostedService: LockedSeatsList count={_seatLockingService.LockedList.Count}");
                        await DeleteExpiredReservation(_seatLockingService.LockedList);
                        await Task.Delay(TimeSpan.FromSeconds(60), cancellationToken);
                    }
                    catch (OperationCanceledException) { }
                }
            }, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private async Task DeleteExpiredReservation(List<SeatLockingModel> lockedList)
        {
            var actualDate = _dateTime.Now;
            var items = lockedList.Where(x => x.ExpirationTime.AddMinutes(1) < actualDate);
            items.ToList().ForEach(async i =>
            {
                using (var scope = scopeFactory.CreateScope())
                {
                    var _context = scope.ServiceProvider.GetRequiredService<ICinemaDbContext>();
                    var itemToUpdate =
                        _context.SeanceSeats.FirstOrDefault(x => x.Id == i.SeanceSeatId && x.BookingId == null);
                    if (itemToUpdate != null)
                    {
                        itemToUpdate.SeatStatus = false;
                        _context.SeanceSeats.Update(itemToUpdate);
                        await _context.SaveChangesAsync(CancellationToken.None);
                    }
                }
            });
            lockedList.RemoveAll(item => item.ExpirationTime.AddMinutes(1) < actualDate);

        }
    }
}
