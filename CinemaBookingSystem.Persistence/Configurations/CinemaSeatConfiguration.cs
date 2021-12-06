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
    public class CinemaSeatConfiguration : IEntityTypeConfiguration<CinemaSeat>
    {
        public void Configure(EntityTypeBuilder<CinemaSeat> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.SeatNumber).IsRequired();
            builder.Property(x => x.Row).IsRequired();
            builder.Property(x => x.SeatType).IsRequired();
            builder.Property(x => x.CinemaHallId).IsRequired();
        }
    }
}
