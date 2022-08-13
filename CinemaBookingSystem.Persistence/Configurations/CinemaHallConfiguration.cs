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
    public class CinemaHallConfiguration : IEntityTypeConfiguration<CinemaHall>

    {
        public void Configure(EntityTypeBuilder<CinemaHall> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(128);
            builder.Property(x => x.TotalSeats).IsRequired();
            builder.Property(x => x.CinemaId).IsRequired();
        }
    }
}
