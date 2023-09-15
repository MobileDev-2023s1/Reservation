using Group_BeanBooking.Areas.Identity.Data;
using Group_BeanBooking.Data;
using Microsoft.AspNetCore.Identity;

namespace Group_BeanBooking.Services
{
    public class UserServices : ServicesArea
    {
        public UserServices(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> rolesManager) : base(context, userManager, rolesManager)
        {
           
        }

        #region User Database Queries

        #endregion


    }
}
