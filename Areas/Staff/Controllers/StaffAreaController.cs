using Group_BeanBooking.Areas.Identity.Data;
using Group_BeanBooking.Data;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Group_BeanBooking.Areas.Staff.Controllers
{
    [Area("Staff")]
    public class StaffAreaController : AreasController
    {
        public StaffAreaController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> rolesManager)
            : base(context, userManager, rolesManager)
        {
            
        }
    }
}
