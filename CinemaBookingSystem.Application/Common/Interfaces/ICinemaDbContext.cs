using System.Threading;
using System.Threading.Tasks;
using CinemaBookingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingSystem.Application.Common.Interfaces
{
    public interface ICinemaDbContext
    {
        DbSet<Actor> Actors { get; set; }
        DbSet<Booking> Bookings { get; set; }
        DbSet<Cinema> Cinemas { get; set; }
        DbSet<CinemaHall> CinemaHalls { get; set; }
        DbSet<CinemaSeat> CinemaSeats { get; set; }
        DbSet<Director> Directors { get; set; }
        DbSet<Genre> Genres { get; set; }
        DbSet<Movie> Movies { get; set; }
        DbSet<Payment> Payments { get; set; }
        DbSet<Seance> Seances { get; set; }
        DbSet<SeanceSeat> SeanceSeats { get; set; }
        DbSet<UserMovieVote> UserMovieVotes { get; set; }
        DbSet<UserPreferencesMovie> UserPreferencesMovies { get; set; }
        DbSet<UserCluster> UserClusters { get; set; }
        DbSet<UserRecommendationType> UserRecommendationTypes { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
