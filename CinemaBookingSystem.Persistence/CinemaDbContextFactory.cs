using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingSystem.Persistence
{
    public class CinemaDbContextFactory : DbContextFactoryBase<CinemaDbContext>
    {
        protected override CinemaDbContext CreateNewInstance(DbContextOptions<CinemaDbContext> options)
        {
            return new CinemaDbContext(options);
        }
    }
}
