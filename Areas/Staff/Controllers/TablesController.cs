using System.Data.Entity;
using System.Diagnostics.Metrics;

using Group_BeanBooking.Areas.Customers.Models.Bookings;
using Group_BeanBooking.Areas.Identity.Data;
using Group_BeanBooking.Areas.Staff.Data;
using Group_BeanBooking.Areas.Staff.Models.Bookings;
using Group_BeanBooking.Data;
using Group_BeanBooking.Services;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Org.BouncyCastle.Security.Certificates;

namespace Group_BeanBooking.Areas.Staff.Controllers
{
    public class TablesController : StaffAreaController
    {
        private readonly RestaurantServices _restaurantServices;
        private readonly ReservationServices _reservationServices;

        public TablesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> rolesManager)
            : base(context, userManager, rolesManager)
        {
            _restaurantServices = new RestaurantServices(context, userManager, rolesManager);
            _reservationServices = new ReservationServices(context, userManager, rolesManager);
        }

        public WhereClause CreateClause()
        {
            var clause = new WhereClause();/*.BuildCurrentCapacityRestaurantClause(r);*/

            return clause;
        }

        

        [HttpPost]
        public async Task<IActionResult> TablesAvailableInSitting([FromBody] Edit c)
        {
            /* 1) get the list of all tables in the same area where person is sitting*/
           
            var listTablesInArea = await GetListOfTablesInArea(c);

            /*2) get list of bookings that are overlaping the current booking */

            var listOfReservations = await GetListOfBookings(c);

            //3) which tables from the table list are being assigned to the bookings?
            return Ok(AssignAvailabilityToTable(listTablesInArea, listOfReservations));
        }

        [HttpPost]
        public async Task<List<RestaurantTable>> GetListOfTablesInArea([FromBody] Edit c)
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

        [HttpPost]
        public async Task<List<Reservation>> GetListOfBookings(Edit c)
        {
            var clause = CreateClause();
            var r = new Reservation()
            {
                Id = c.ReservationId,
                Duration = c.Duration,
                Start = c.Starttime.AddHours(11),
                RestaurantAreaId = c.RestaurantAreaId,
                ReservationStatusID = c.ReservationStatusId,
            };

            var newClause = clause.BuildListOfBookings(r);
            var list = await _reservationServices.GetAllReservations(newClause);

            return list;
        }

        [HttpPost]
        public List<object> AssignAvailabilityToTable(List<RestaurantTable> listTablesInArea , List<Reservation> listOfReservations)
        {
            var listTables = new List<object>();
            
            foreach (var table in listTablesInArea)
            {
                var e = listOfReservations.Any(res => res.RestaurantTables.Contains(table));

                var status = new TableStatus()
                {
                    Id = table.Id,
                    Name = table.Name,
                    Status = e ? false : true,
                };
                listTables.Add(status);
            }

            return listTables;
        }
    
    
    
    }
}
