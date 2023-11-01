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

        public async Task AddTablesToBooking(Reservation res, List<RestaurantTable> tables, Edit c)
        {
            foreach (var table in c.RestaurantTables)
            {
                var selection = tables.Find(t => t.Id == table.Id);
                var result = res.RestaurantTables.Contains(selection);
                if (!result)
                {
                    res.RestaurantTables.Add(selection);
                    _context.SaveChangesAsync();
                }
            }

        }

        public async Task<Edit> RemoveTableFromBooking(Reservation res, List<RestaurantTable> tables, Edit c)
        {
            if(c.RestaurantTables.Count == 0)
            {
                res.RestaurantTables.Clear();
                
            }else if(c.ReservationStatusId != 2 && c.ReservationStatusId != 4){

                res.RestaurantTables.Clear();
                c.RestaurantTables.Clear();
            }
            else
            {
                var remove= await IdentifyTablesToRemove(res, tables, c);
                
                foreach (var table in remove)
                {
                    res.RestaurantTables.Remove(table);
                }
                                    
            }

            return c;

        }

        public async Task<List<RestaurantTable>> IdentifyTablesToRemove(Reservation res, List<RestaurantTable> tables, Edit c)
        {
            List<RestaurantTable> remove = new List<RestaurantTable>();
            //form the initial list the reservation has
            foreach (var table in res.RestaurantTables)
            {
                var result = c.RestaurantTables.Find(t => t.Id == table.Id);
                if (result == null)
                {
                    var tableToRemove = res.RestaurantTables.Find(t => t.Id == table.Id);
                    remove.Add(tableToRemove);
                }
            }

            return remove;
        }
    }
}
