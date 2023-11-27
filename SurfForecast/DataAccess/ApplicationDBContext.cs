using Microsoft.EntityFrameworkCore;
using SurfForecast.Models;

namespace SurfForecast.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
            base(options)
        { }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Wind> Winds { get; set; }
        public DbSet<Swell> Swells { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        // Configure other entities and relationships as needed...
    }

        
    }
}