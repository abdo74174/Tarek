using Grand_Hall.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Grand_Hall.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        // Constructor to pass options to the base class
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Define the DbSets for your models
        public DbSet<Hall> Halls { get; set; }
        //public DbSet<Reservation> Reservations { get; set; }
        //public DbSet<Booker> Bookers { get; set; }

        // Customize the model if needed (e.g., configure relationships, table names, etc.)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Example of adding custom configuration for models
            modelBuilder.Entity<Hall>().ToTable("Halls", "EventHall");
            //modelBuilder.Entity<Reservation>().ToTable("Reservations", "EventHall");
            //modelBuilder.Entity<Booker>().ToTable("Bookers", "EventHall");
        }
    }
}
