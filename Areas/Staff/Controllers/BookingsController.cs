using Group_BeanBooking.Areas.Identity.Data;
using Group_BeanBooking.Areas.Staff.Models.Bookings;
using Group_BeanBooking.Data;
using Group_BeanBooking.Services;

using Humanizer;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
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
        public BookingsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> rolesManager) : base(context, userManager, rolesManager)
        {
            _restaurantServices = new RestaurantServices(context, userManager, rolesManager);
            _personServices = new PersonServices(context, userManager, rolesManager);
            _reservationServices = new ReservationServices(context, userManager, rolesManager);
        }

        
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Details()
        {

            //var c = new Group_BeanBooking.Areas.Staff.Models.Details();
            return View();
            
        }

        [HttpGet]
        public async Task<List<LoadDetails>> GetAllReservations()
        {
            var model = new List<LoadDetails>();

            var reservations = await _reservationServices.GetAllReservations();

            foreach (var r in reservations)
            {
                model.Add(new LoadDetails
                {
                    BookingId = r.Id,
                    Start = r.Start,
                    Name = r.Person.FirtName.ToString() + " " + r.Person.LastName.ToString(),
                });
            }

            return model;
        }

        [HttpGet]
        public async Task<IActionResult> GetReservations(string start)
        {
            var current = DateTime.Parse(start);
            //var current = DateTime.Now;
            var startDate = current.AddDays(-current.Day).AtMidnight();
            var endDate = startDate.AddDays(Days(current));

            var reservations = await _reservationServices.GetActiveReservationsByMonth(startDate, endDate);

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



    }
}
