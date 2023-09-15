using Group_BeanBooking.Areas.Identity.Data;
using Group_BeanBooking.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Group_BeanBooking.Data;

namespace Group_BeanBooking.Services
{
    public class RestaurantServices : ServicesArea
    {
        public RestaurantServices(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> rolesManager) : base(context, userManager, rolesManager)
        {
            
        }

        #region Contains DB queries for Restaurants
            public async Task<Restaurant> GetRestaurantByID(int id)
            {
                return await _context.Restaurants.FirstOrDefaultAsync(r => r.Id == id);
            }

            public async Task<List<RestaurantArea>> GetRestaurantAreaByRestaurantId(int restaurantId)
            {
                return await _context.ResturantAreas.Where(r => r.RestaurantId == restaurantId).ToListAsync();
            }

            public async Task<List<Sitting>> GetSittingsByRestaurantId(int id)
            {
                return await _context.Sittings.Where(r => r.RestaurantId == id).ToListAsync();
            }

            public async Task<List<Restaurant>> GetRestaurants()
            {
                return await _context.Restaurants.ToListAsync();
            }

            public async Task<List<RestaurantArea>> GetRestaurantAreas()
            {
                return await _context.ResturantAreas.ToListAsync();
            }

            public async Task<List<Sitting>> GetSittings()
            {
                return await _context.Sittings.ToListAsync();
            }

        #endregion
    }
}
