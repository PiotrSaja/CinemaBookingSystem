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
    public class UserClusterConfiguration : IEntityTypeConfiguration<UserCluster>
    {
        public void Configure(EntityTypeBuilder<UserCluster> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
