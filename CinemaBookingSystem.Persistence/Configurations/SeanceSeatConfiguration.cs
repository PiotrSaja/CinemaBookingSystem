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
    public class SeanceSeatConfiguration : IEntityTypeConfiguration<SeanceSeat>
    {
        public void Configure(EntityTypeBuilder<SeanceSeat> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Price).IsRequired();
            builder.Property(x => x.SeatStatus).IsRequired();
        }
    }
}
