using Group_BeanBooking.Areas.Identity.Data;
using Group_BeanBooking.Data;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Group_BeanBooking.Services
{
    public class TableServices : ServicesArea
    {
        public TableServices(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
           RoleManager<IdentityRole> rolesManager) : base(context, userManager, rolesManager)
        {

        }

       
    }
}
