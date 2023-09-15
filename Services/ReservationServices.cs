using Group_BeanBooking.Areas.Customers.Models.Bookings;
using Group_BeanBooking.Areas.Identity.Data;
using Group_BeanBooking.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Group_BeanBooking.Data;

namespace Group_BeanBooking.Services
{
    public class ReservationServices : ServicesArea
    {
        public ReservationServices(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> rolesManager) : base(context, userManager, rolesManager)
        {

        }

        #region DB queries for reservations
        public async Task<Reservation> CreateReservation(Person person, Create c)
        {
            var reservation = new Reservation
            {
                Start = c.Starttime,
                Duration = c.Duration,
                PersonId = person.Id,
                SittingID = c.SittingId,
                ReservationStatusID = 1,
                ResevationOrigin = await GetResevationOrigins("online"),
                Guests = c.Guests,
            };

            await _context.Reservations.AddAsync(reservation);
            await _context.SaveChangesAsync();

            return reservation;

        }

        public async Task<ResevationOrigin> GetResevationOrigins(string reservationOriginName)
        {
            return await _context.ResevationOrigins.FirstOrDefaultAsync(r => r.Name == reservationOriginName);
        }

        public async Task<List<Reservation>> GetReservationsByPersonId(int? personId)
        {
            return await _context.Reservations.Where(r => r.PersonId == personId).ToListAsync();
        }

        //https://learn.microsoft.com/en-us/ef/core/querying/related-data/eager
        public async Task<List<Reservation>> GetReservations(int personId)
        {
            var reservations = await _context.Reservations
                .Include(r => r.Person) //eager loading
                .Include(r => r.Sitting) //keyless entities mapping them to the result set of store procedure
                    .ThenInclude(s => s.Restaurant)
                .Include(r => r.ResevationOrigin)
                .Include(r => r.ReservationStatus)
                .Where(p => p.PersonId == personId).ToListAsync();

            return reservations;
        }

        #endregion

    }
}
