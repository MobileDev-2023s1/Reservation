using Group_BeanBooking.Areas.Customers.Models.Bookings;
using Group_BeanBooking.Data;
using ReservationSystem.Data;

namespace Group_BeanBooking.Areas.Customers.Data
{
    public class Bookings
    {
        private readonly ApplicationDbContext _context;
        private readonly Queries _queries;

        public Bookings(ApplicationDbContext context)
        {
            _context = context;
            _queries = new Queries(context);
        }

        public Reservation CreateReservation(Person person, Create c)
        {
            var reservation = new Reservation
            {
                Start = c.Starttime,
                Duration = c.Duration,
                PersonId = person.Id,
                SittingID = c.SittingId,
                ReservationStatusID = 1,
                ResevationOrigin = _queries.GetResevationOrigins("online"),
                Guests = c.Guests,
            };

            _context.Reservations.Add(reservation);
            _context.SaveChanges();

            return reservation;

        }
    }
}
