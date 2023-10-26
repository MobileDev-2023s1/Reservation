using System.Data.Entity;
using System.Linq.Expressions;

using Group_BeanBooking.Areas.Identity.Data;
using Group_BeanBooking.Areas.Staff.Data;
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

        public async Task<List<RestaurantTable>> GetTablesById(Expression<Func<RestaurantTable, bool>> clause)
        {
            return await _context.RestaurantTables.ToListAsync();

        }
       
    }
}
