using System.Data.Entity;
using System.Linq.Expressions;

using Group_BeanBooking.Areas.Customers.Models.Bookings;
using Group_BeanBooking.Areas.Identity.Data;
using Group_BeanBooking.Areas.Staff.Data;
using Group_BeanBooking.Areas.Staff.Models.Bookings;
using Group_BeanBooking.Data;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Group_BeanBooking.Services
{
    public class TableServices : ServicesArea
    {
        private readonly RestaurantServices _restaurantServices;
        
        public TableServices(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
           RoleManager<IdentityRole> rolesManager) : base(context, userManager, rolesManager)
        {
            _restaurantServices = new RestaurantServices(context, userManager, rolesManager);
        }

        public WhereClause CreateClause()
        {
            var clause = new WhereClause();/*.BuildCurrentCapacityRestaurantClause(r);*/

            return clause;
        }

        public async Task<List<RestaurantTable>> GetListOfTablesInArea(Edit c)
        {
            var clause = CreateClause();
            var table = new RestaurantTable()
            {
                RestaurantAreaId = c.RestaurantAreaId
            };

            var newClause = clause.BuildRestaurantTableClause(table);

            var list = await _restaurantServices.GetListofTables(newClause);

            return list;

        }
    }
}
