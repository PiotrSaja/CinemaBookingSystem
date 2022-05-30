using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CinemaBookingSystem.Application.Common.Interfaces;
using CinemaBookingSystem.Domain.Common;
using CinemaBookingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Converters;

namespace CinemaBookingSystem.Persistence
{
    public class CinemaDbContext : DbContext, ICinemaDbContext
    {
        private readonly IDateTime _dateTime;
        private readonly IUserService _userService;
        public CinemaDbContext(DbContextOptions<CinemaDbContext> options) : base(options)
        {
        }
        public CinemaDbContext(DbContextOptions<CinemaDbContext> options, IDateTime dateTime, IUserService userService) : base(options)
        {
            _dateTime = dateTime;
            _userService = userService;
        }

        public DbSet<Actor> Actors { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<CinemaHall> CinemaHalls { get; set; }
        public DbSet<CinemaSeat> CinemaSeats { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Payment> Payments{ get; set; }
        public DbSet<Seance> Seances{ get; set; }
        public DbSet<SeanceSeat> SeanceSeats{ get; set; }
        public DbSet<UserMovieVote> UserMovieVotes { get; set; }
        public DbSet<UserPreferencesMovie> UserPreferencesMovies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Actor>().OwnsOne(a => a.ActorName);
            modelBuilder.Entity<Booking>().OwnsOne(b=>b.PersonalName);
            modelBuilder.Entity<Cinema>().OwnsOne(c=>c.Address);
            modelBuilder.Entity<Director>().OwnsOne(d => d.DirectorName);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _userService.Email;
                        entry.Entity.Created = _dateTime.Now;
                        entry.Entity.StatusId = 1;
                        break;
                    case EntityState.Modified:
                        entry.Entity.ModifiedBy = _userService.Email;
                        entry.Entity.Modified = _dateTime.Now;
                        break;
                    case EntityState.Deleted:
                        entry.Entity.ModifiedBy = _userService.Email;
                        entry.Entity.Modified = _dateTime.Now;
                        entry.Entity.Inactivated = _dateTime.Now;
                        entry.Entity.InactivatedBy = _userService.Email;
                        entry.Entity.StatusId = 0;
                        entry.State = EntityState.Modified;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
