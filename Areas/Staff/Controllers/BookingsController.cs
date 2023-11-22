using System.Diagnostics.Contracts;
using System.Linq.Expressions;

using Google.Protobuf.WellKnownTypes;

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
using Microsoft.Build.Construction;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

using NuGet.Packaging.Signing;

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
        private readonly TableServices _tableServices;
        public BookingsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> rolesManager) : base(context, userManager, rolesManager)
        {
            _restaurantServices = new RestaurantServices(context, userManager, rolesManager);
            _personServices = new PersonServices(context, userManager, rolesManager);
            _reservationServices = new ReservationServices(context, userManager, rolesManager);
            _statusesServices = new StatusesServices(context, userManager, rolesManager);
            _tableServices = new TableServices(context, userManager, rolesManager);
            
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
        public async Task<IActionResult> GetReservations(string start, string? email, int location, int status)
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
            
            var clause = whereClause.BuildCalendarViewReservationClause(whereClause);

            //if status id = null // load all and exclude 3 and 5 
            if(whereClause.StatusId == 0)
            {
                reservations = await _reservationServices.GetActiveReservationsByMonth(clause);
            }
            else
            {
                reservations = await _reservationServices.GetReservationByStatusID(clause);
            }

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
            var whereClause = new WhereClause
            {
                BookingId = bookingID,
            };
            var clause = whereClause.BuildCalendarViewReservationClause(whereClause);
            var r = await _reservationServices.GetSingelReservationById(clause);
            
            
            var model = new LoadDetails()
            {
                BookingId = r.Id,
                RestaurantID = r.Sitting.RestaurantId,
                Duration = r.Duration,
                SittingId = r.Sitting.Id,
                StartTime = r.Start,
                EndTime = r.End,
                AssignedTable = r.RestaurantTables,
                Guest = r.Guests,
                PersonId = r.Person.Id,
                PersonName = string.Concat(r.Person.FirtName, " ", r.Person.LastName),
                RestaurantAreaId = r.RestaurantAreaId,
                ReservationStatusId = r.ReservationStatus.Id,
                Comments = r.Comments,
                SittingName = r.Sitting.Name,
                RestaurantAreaName = r.RestaurantArea.Name,
                //SittingAreaList = new SelectList(sittingAreaList, "Id", "Name")

            };
            
            return Ok(model);
        }

        //[HttpPost("UpdateBookingDetails", Name = "UpdateBooking")]
        [HttpPost]
        public async Task<IActionResult> NewBookingDetails([FromBody]Edit c)
        {
            if (ModelState.IsValid)
            {                
                //find the booking
                var res = await _reservationServices.GetReservationsByReservationId(c.ReservationId);
                var clause = new WhereClause().BuildRestaurantTableClause(new RestaurantTable() { RestaurantAreaId = c.RestaurantAreaId });
                var tables =  await _restaurantServices.GetListofTables(clause);

                //remove tables from booking
                c = await _tableServices.RemoveTableFromBooking(res, tables, c);


                //add tables to booking
                await _tableServices.AddTablesToBooking(res, tables, c);

                //save the changes
                c.Starttime = c.Starttime.AddHours(11);
                await _reservationServices.EditReservation(c);
                await _context.SaveChangesAsync();
                TempData["AlertMessage"] = "Booking modified successfully";
            }
            return Ok($"Booking id {c.ReservationId} updated");
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
                //cancelled
                case 3: return "#ffd000"; break;
                //Completed
                case 5: return "#52b788"; break;
                //Cancellation requested
                case 6: return "#ff4d6d"; break;
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
                //cancelled
                case 3: return "#ffd000"; break;
                //Completed
                case 5: return "#52b788"; break;
                //Cancellation requested
                case 6: return "#ff4d6d"; break;
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
                //cancelled
                case 3: return "#ff7b00"; break;
                //seated
                case 4: return "#084298"; break;
                //Completed
                case 5: return "#081c15"; break;
                //Cancellation requested
                case 6: return "#590d22"; break;
                default: return null;

            }
        }

        #endregion



    }
}
