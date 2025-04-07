using Microsoft.EntityFrameworkCore;

namespace POEPart1_VenueBookingSystem.Models
{
    public class AppDBContext : DbContext 
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options){ }

        public DbSet<Venue> Venue { get; set; }
        public DbSet<Event> Event { get; set; }
        public DbSet<Booking> Booking { get; set; }

        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Ensure the correct table name is used
            modelBuilder.Entity<Venue>().ToTable("Venue");
            base.OnModelCreating(modelBuilder);
        }*/
    }
}
