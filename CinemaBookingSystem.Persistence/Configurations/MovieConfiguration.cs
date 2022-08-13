using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaBookingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaBookingSystem.Persistence.Configurations
{
    public class MovieConfiguration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title).IsRequired().HasMaxLength(256);
            builder.Property(x => x.Released).IsRequired();
            builder.Property(x => x.Duration).IsRequired();
            builder.Property(x => x.Plot).IsRequired();
            builder.Property(x => x.Country).IsRequired().HasMaxLength(256);
            builder.Property(x => x.Language).IsRequired();
            builder.Property(x => x.PosterPath).IsRequired();
            builder.Property(x => x.ImdbRating).IsRequired();
        }
    }
}
