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

        public ApplicationUser GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public ApplicationUser GetUserById(string id)
        {
            return _context.Users.FirstOrDefault(r => r.Id == id);
        }
      
        public List<Reservation> GetReservationsByPersonId(int personId)
        {
            return _context.Reservations.Where(r => r.PersonId == personId).ToList();
        }

        public List<Reservation> GetReservations(int personId)
        {
            var query = _context.Reservations
                .Join(_context.Sittings, r => r.SittingID, s => s.Id, (r, s) => new { Name = s.Restaurant.Name });
                
            

            //var value2 = from r in _context.Reservations
            //             join s in _context.Sittings on r.SittingID equals s.Id
            //             where r.PersonId == personId
            //             select new
            //             {
            //                 Name = s.Restaurant.Name
            //             };

            return _context.Reservations.Where(r => r.PersonId == personId).ToList();
        }


    }
}
