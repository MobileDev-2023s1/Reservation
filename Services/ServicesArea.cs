using Group_BeanBooking.Areas.Identity.Data;
using Group_BeanBooking.Areas;
using Group_BeanBooking.Data;
using Microsoft.AspNetCore.Identity;

namespace Group_BeanBooking.Services
{
    public class ServicesArea
    {
        protected readonly ApplicationDbContext _context;
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly RoleManager<IdentityRole> _rolesManager;

        public ServicesArea(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> rolesManager)
        {
            _context = context;
            _userManager = userManager;
            _rolesManager = rolesManager;
        }


    }
}
