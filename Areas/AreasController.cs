using Group_BeanBooking.Areas.Identity.Data;
using Group_BeanBooking.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Group_BeanBooking.Data;

namespace Group_BeanBooking.Areas
{
    public class AreasController : Controller
    {
        //protected readonly List<Restaurant> _restaurants;
        //protected readonly List<RestaurantArea> _restaurantArea;
        //protected readonly List<SittingType> _sittingTypes;
        //protected readonly List<Sitting> _sittings; //not good practice
        //protected readonly List<RestaurantTable> _tables;
        protected readonly ApplicationDbContext _context;
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly RoleManager<IdentityRole> _rolesManager;

        public AreasController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> rolesManager)
        {
            _context = context;
            _userManager = userManager;
            _rolesManager = rolesManager;
            //_restaurants = _context.Restaurants.ToList();
            //_restaurantArea = _context.ResturantAreas.ToList();
            //_sittingTypes = _context.SittingTypes.ToList();
            //_sittings = _context.Sittings.ToList();
            //_tables = _context.RestaurantTables.ToList();
        }
        
    }
}
