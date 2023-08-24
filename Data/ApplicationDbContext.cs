using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReservationSystem.Data;

namespace Group_BeanBooking.Data
{
    public class ApplicationDbContext : IdentityDbContext
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
    }
}