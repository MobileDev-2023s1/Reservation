using Group_BeanBooking.Areas.Identity.Data;
using Group_BeanBooking.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Group_BeanBooking.Areas.Customers.Controllers
{

    public class BookingsController : CustomersAreaController
    {
        public BookingsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> rolesManager) : base(context, userManager, rolesManager)
        {
            
        }
        //data that
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create(int id)
        {
            var restInfo = 
            var areas = _context.ResturantAreas.Where(r => r.RestaurantId == id).ToList();
            var sittings = _context.Sittings.Where(r => r.RestaurantId == id).ToList();
            var c = new Group_BeanBooking.Areas.Customers.Models.Bookings.Create()
            {
                
                SittingAreaList = new SelectList(areas, "Id", "Name"),
                SittingList = new SelectList(sittings, "Id", "Name"),

            };
            

            return View(c);
        }

        [HttpPost]
        public IActionResult Create(Group_BeanBooking.Areas.Customers.Models.Bookings.Create c)
        {


            return View();
        }


    }
}
