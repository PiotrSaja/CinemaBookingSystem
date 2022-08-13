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
    public class CinemaConfiguration : IEntityTypeConfiguration<Cinema>
    {
        public void Configure(EntityTypeBuilder<Cinema> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(128);
            builder.Property(x => x.TotalCinemaHalls).IsRequired();

            builder.OwnsOne(x => x.Address).Property(x => x.State).IsRequired().HasMaxLength(64);
            builder.OwnsOne(x => x.Address).Property(x => x.City).IsRequired().HasMaxLength(64);
            builder.OwnsOne(x => x.Address).Property(x => x.Country).IsRequired().HasMaxLength(64);
            builder.OwnsOne(x => x.Address).Property(x => x.Street).IsRequired().HasMaxLength(128);
            builder.OwnsOne(x => x.Address).Property(x => x.ZipCode).IsRequired().HasMaxLength(16);
        }
    }
}
