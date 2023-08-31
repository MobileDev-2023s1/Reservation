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

        public IActionResult Create()
        {
            
            return View();
        }
    }
}
