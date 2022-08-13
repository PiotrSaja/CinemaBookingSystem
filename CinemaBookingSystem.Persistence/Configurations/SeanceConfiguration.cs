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
    public class SeanceConfiguration : IEntityTypeConfiguration<Seance>

    {
        public void Configure(EntityTypeBuilder<Seance> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Date).IsRequired();
            builder.Property(x => x.CinemaHallId).IsRequired();
            builder.Property(x => x.MovieId).IsRequired();
            builder.Property(x => x.SeanceType).IsRequired();
        }
    }
}
