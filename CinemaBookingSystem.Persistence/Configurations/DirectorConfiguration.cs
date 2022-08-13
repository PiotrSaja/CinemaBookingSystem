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
    public class DirectorConfiguration : IEntityTypeConfiguration<Director>
    {
        public void Configure(EntityTypeBuilder<Director> builder)
        {
            builder.HasKey(x => x.Id);

            builder.OwnsOne(x => x.DirectorName).Property(x => x.FirstName).IsRequired();
            builder.OwnsOne(x => x.DirectorName).Property(x => x.LastName).IsRequired();
        }
    }
}
