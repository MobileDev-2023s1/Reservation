using Group_BeanBooking.Areas.Customers.Models.Bookings;
using Group_BeanBooking.Areas.Identity.Data;
using Group_BeanBooking.Areas.Staff.Data;
using Group_BeanBooking.Areas.Staff.Models.Bookings;

using Group_BeanBooking.Data;
using Group_BeanBooking.Services;

using Humanizer;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

using Org.BouncyCastle.Bcpg.OpenPgp;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Group_BeanBooking.Areas.Staff.Controllers
{
    public class BookingsController : StaffAreaController
    {
        private readonly RestaurantServices _restaurantServices;
        private readonly PersonServices _personServices;
        private readonly ReservationServices _reservationServices;
        private readonly StatusesServices _statusesServices;
        public BookingsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> rolesManager) : base(context, userManager, rolesManager)
        {
            _restaurantServices = new RestaurantServices(context, userManager, rolesManager);
            _personServices = new PersonServices(context, userManager, rolesManager);
            _reservationServices = new ReservationServices(context, userManager, rolesManager);
            _statusesServices = new StatusesServices(context, userManager, rolesManager);
            
        }

        
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Details()
        {

            var restaurants = await _restaurantServices.GetRestaurants();
            var statuses = await  _statusesServices.GetListReservationStatus();

            var model = new LoadDetails
            {
                RestaurantList = new SelectList(restaurants, "Id", "Name"),
                StatusesList = new SelectList(statuses, "Id", "Name"),
            };
            
            return View(model);
            
        }

        
        [HttpGet]
        public async Task<IActionResult> GetReservations(string start, string email, int location, int status)
        {
            //transform date from calendar to DateTime format
            var current = DateTime.Parse(start);
            var startDate = current.AddDays(-current.Day).AtMidnight();
            var endDate = startDate.AddDays(Days(current));

            List<Reservation> reservations = new List<Reservation>();

            //Creates an object to build the else clause
           var whereClause = new WhereClause
            {
                StartDate = startDate,
                EndDate = endDate,
                Email = email,
                RestaurantId = location,
                StatusId = status
           };

            var clause = whereClause.BuildWhereClause(whereClause);

            if(clause != null) {reservations = await _reservationServices.GetAllReservations(startDate, endDate, clause);}
            else { reservations = await _reservationServices.GetActiveReservationsByMonth(startDate, endDate);}

            var result = reservations.Select(r => new
            {
                id= r.Id,
                title = r.Person.FirtName.ToString() + " " + r.Person.LastName.ToString() + " - Guests: " + r.Guests.ToString(),
                start = r.Start,
                end = r.End,
                backgroundColor = AssignBackgroundColor(r.ReservationStatusID),
                borderColor = AssignBorderColor(r.ReservationStatusID),
                textColor = AssignTextColor(r.ReservationStatusID),
            });

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetReservationById(int bookingID)
        {
            var model = new CompleteDetails();
            var clause = new WhereClause
            {
                BookingId = bookingID,
            }

            var reservation = _reservationServices.GetAllReservations(clause)

            return View();
        }



        #region Includes format information for events on the calendar
        public int Days(DateTime date)
        {
            switch (date.Month)
            {
                case 1: case 3: case 5: case 7: case 8: case 10: case 12: return 31; break;
                case 4: case 6: case 9: case 11: return 30; break;
                case 2: 
                if(DateTime.IsLeapYear(date.Year))
                {
                    return 29; 
                } else
                {
                    return 28; 
                }
                break;
            }

            return 0;
        }

        public string AssignBackgroundColor(int id)
        {
            switch (id)
            {
                //pending
                case 1: return "#f8d7da"; break;
                //confirmed
                case 2: return "#d1e7dd"; break;
                //seated
                case 4: return "#cfe2ff"; break;
                default: return null;


            }
        }

        public string AssignBorderColor(int id)
        {
            switch (id)
            {
                //pending
                case 1: return "#f5c2c7"; break;
                //confirmed
                case 2: return "#badbcc"; break;
                //seated
                case 4: return "#b6d4fe"; break;
                default: return null;

            }
        }

        public string AssignTextColor(int id)
        {
            switch (id)
            {
                //pending
                case 1: return "#842029"; break;
                //confirmed
                case 2: return "#0f5132"; break;
                //seated
                case 4: return "#084298"; break;
                default: return null;

            }
        }

        #endregion



    }
}
