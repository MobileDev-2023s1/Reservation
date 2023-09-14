using Group_BeanBooking.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;
using ReservationSystem.Data;
using System.Linq;


namespace Group_BeanBooking.Data
{
    public class Queries
    {
        private readonly ApplicationDbContext _context;

        public Queries(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Restaurant> GetRestaurants()
        {
            return _context.Restaurants.ToList();
        }

        public Restaurant GetRestaurantByID(int id)
        {
            return _context.Restaurants.FirstOrDefault(r=> r.Id == id);
        }

        public List<RestaurantArea> GetRestaurantAreas()
        {
            return _context.ResturantAreas.ToList();
        }

        public List<RestaurantArea> GetRestaurantAreaByRestaurantId(int restaurantId)
        {
            return _context.ResturantAreas.Where(r => r.RestaurantId == restaurantId).ToList();
        }

        public List<Sitting> GetSittings()
        {
            return _context.Sittings.ToList();
        }

        public List<Sitting> GetSittingsByRestaurantId(int id)
        {
            return _context.Sittings.Where(r => r.RestaurantId == id).ToList();
        }

        public Person GetPersonById(string UserId)
        {
            if (UserId == null)
            {
                return null;
            }
            {
                return _context.People.FirstOrDefault(p => p.UserId.Equals(UserId));
            }
        }

        public Person GetPersonByEmail(string email) 
        {
            return _context.People.FirstOrDefault(u => u.Email == email);
        }

        public ResevationOrigin GetResevationOrigins(string reservationOriginName)
        {
            return _context.ResevationOrigins.FirstOrDefault(r=> r.Name == reservationOriginName);
        }

        public ApplicationUser GetUserByEmail(string? email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public ApplicationUser GetUserById(string id)
        {
            return _context.Users.FirstOrDefault(r => r.Id == id);
        }
      
        public List<Reservation> GetReservationsByPersonId(int? personId)
        {
            return _context.Reservations.Where(r => r.PersonId == personId).ToList();
        }


        //https://learn.microsoft.com/en-us/ef/core/querying/related-data/eager
        public List<Reservation> GetReservations(int personId)
        {
            var reservations = _context.Reservations
                .Include(r => r.Person) //eager loading
                .Include(r => r.Sitting) //keyless entities mapping them to the result set of store procedure
                    .ThenInclude(s => s.Restaurant)
                .Include(r => r.ResevationOrigin)
                .Include(r => r.ReservationStatus)
                .Where(p => p.PersonId == personId).ToList();

            return reservations;
        }


    }
}
