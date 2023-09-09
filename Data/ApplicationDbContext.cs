using Group_BeanBooking.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Group_BeanBooking.Data;
using System.Reflection.Metadata;

namespace Group_BeanBooking.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Person> People { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ReservationStatus> ReservationStatuses { get; set; }
        public DbSet<ResevationOrigin> ResevationOrigins { get; set; }

        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<RestaurantArea> ResturantAreas { get; set; }
        public DbSet<RestaurantTable> RestaurantTables { get; set; }
        public DbSet<Sitting> Sittings { get; set; }
        public DbSet<SittingType> SittingTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<Sitting>()
                .HasMany(s => s.Reservations)
                .WithOne(r => r.Sitting)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<Person>()
                .Property(p => p.Email).HasMaxLength(150);

            modelBuilder
                .Entity<Person>()
                .HasIndex(p => p.Email).IsUnique(true);

            modelBuilder
                .Entity<Person>()
                .Property(p=> p.UserId).HasMaxLength(450);

            //modelBuilder
            //    .Entity<Person>()
            //    .HasOne<IdentityUser>()
            //    .WithOne().IsRequired(false).HasForeignKey("IdentityUser", new[] { "UserId" });

            modelBuilder
               .Entity<Person>()
               .HasOne(p => p.User)
               .WithOne().IsRequired(false);

            
        }
    }
}