using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Metrics;

using Google.Protobuf.WellKnownTypes;

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
        private readonly TableServices _tableServices;

        public TablesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> rolesManager)
            : base(context, userManager, rolesManager)
        {
            _restaurantServices = new RestaurantServices(context, userManager, rolesManager);
            _reservationServices = new ReservationServices(context, userManager, rolesManager);
            _tableServices = new TableServices(context, userManager, rolesManager); 
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
           
            var listTablesInArea = await _tableServices.GetListOfTablesInArea(c);

            /*2) create reservation and pass only Id */
            var reservation = new Reservation()
            {
                Id = c.ReservationId,
                Start = c.Starttime.AddHours(11),
                Duration = c.Duration,
                SittingID = c.SittingId
            };

            //3) which tables from the table list are being assigned to the bookings?
            return Ok(AssignAvailabilityToTable(listTablesInArea, reservation));
        }

        public List<object> AssignAvailabilityToTable(List<RestaurantTable> listTablesInArea, Reservation reservation)
        {
            var listTables = new List<object>();

            foreach (var table in listTablesInArea)
            {
                var result = table.Reservations
                    //.Any(item => item.Id == reservation.Id);
                    .Where(item => item.End >= reservation.Start && item.SittingID == reservation.SittingID)
                    .FirstOrDefault();

                var status = new TableStatus()
                {
                    Id = table.Id,
                    Name = table.Name,
                    Status = result != null ? false : true,
                    ReservationId = result != null ? result.Id : 0,
                };
                listTables.Add(status);
            }

            return listTables;
        }

    }
}
