using Group_BeanBooking.Areas.Identity.Data;
using Group_BeanBooking.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
            
            return View();
        }

        [HttpPost]
        public IActionResult Create(Group_BeanBooking.Areas.Customers.Models.Bookings.Create c)
        {


            return View();
        }


    }
}
