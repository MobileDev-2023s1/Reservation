using Group_BeanBooking.Areas.Identity.Data;
using Group_BeanBooking.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Group_BeanBooking.Areas.Customers.Controllers
{
    [Area("Customers")]
    //[Authorize(Roles = "Customer,Owner")] //if I need to add more roles
    public class CustomersAreaController : AreasController
    {
        public CustomersAreaController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> rolesManager) 
            : base(context, userManager, rolesManager)
        {
        }
    }
}
