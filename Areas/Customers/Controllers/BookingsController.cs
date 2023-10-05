using Group_BeanBooking.Areas.Identity.Data;
using Group_BeanBooking.Data;
using Group_BeanBooking.Areas.Customers.Models.Bookings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Group_BeanBooking.Services;
using Microsoft.EntityFrameworkCore;
using Humanizer;
using Microsoft.VisualBasic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Group_BeanBooking.Areas.Customers.Controllers
{

    public class BookingsController : CustomersAreaController
    {
        
        private readonly RestaurantServices _restaurantServices;
        private readonly PersonServices _personServices;
        private readonly ReservationServices _reservationServices;

        public BookingsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> rolesManager) : base(context, userManager, rolesManager)
        {
            
            _restaurantServices = new RestaurantServices(context, userManager,rolesManager);
            _personServices = new PersonServices(context, userManager,rolesManager);
            _reservationServices = new ReservationServices(context, userManager,rolesManager);  
        }
        
        public IActionResult Index()
        {
            return View();
        }

        #region API's for CRUD operations

        [HttpGet]
        public async Task<IActionResult> Create(int id)
        {
            var restaurant = await _context.Restaurants
                .Include(s => s.Sittings)
                .Include(a => a.RestaurantAreas)
                .SingleOrDefaultAsync(r => r.Id == id);

            var c = new Create()
            {
                RestaurantId = id,
                RestaurantName = restaurant.Name,
                SittingAreaList = new SelectList(restaurant.RestaurantAreas, "Id", "Name"),
                SittingList = new SelectList(restaurant.Sittings, "Id", "Name")
            };
            
            return View(c);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Group_BeanBooking.Areas.Customers.Models.Bookings.Create c)
        {
            var person = await _personServices.UserValidation(c, null);

            if(ModelState.IsValid)
            {
                await _reservationServices.CreateReservation(person, c);
                TempData["AlertMessage"] = "Booking created successfully";
            }

            return RedirectToAction("Details", "Bookings" , new  { id = person.Id , area="Customers"});
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            var model = new Group_BeanBooking.Areas.Customers.Models.Bookings.Details();
            if(id != null)
            {
                model.Reservations = await _reservationServices.GetReservationsByPersonId(id);
            } else if(User.Identity.IsAuthenticated)
            {
                var person = await _context.People.SingleOrDefaultAsync(u => u.Email == User.Identity.Name);
                model.Reservations = await _reservationServices.GetReservationsByPersonId(person.Id);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Details(string? email , int? id) 
        {
            var model = new Group_BeanBooking.Areas.Customers.Models.Bookings.Details();
            Person user = new();
            List<Reservation> bookings = new();           

            if (ModelState.IsValid)
            {
                //person has not logged in and then email is used for search
                if(id == null)
                {   var u = await _personServices.GetPersonByEmail(email);

                    if(u != null) {bookings = await _reservationServices.GetReservationsByPersonId(u.Id);}
                }
                else
                {
                    bookings = await _reservationServices.GetReservationsByPersonId(id);
                }
            }           

            model.Reservations = bookings;
            model.Person = user;
            model.Email = email;
            
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var reservation = await _reservationServices.GetReservationsByReservationId(id);
            var areas = await _restaurantServices.GetRestaurantAreaByRestaurantId(reservation.Sitting.RestaurantId);
            var sittings = await _restaurantServices.GetSittingsByRestaurantId(reservation.Sitting.RestaurantId);

            var model = new Edit
            {
                ReservationId = reservation.Id,
                RestaurantId = reservation.Sitting.RestaurantId,
                RestaurantName = reservation.Sitting.Restaurant.Name,
                PersonId = reservation.Person.Id,
                FirstName = reservation.Person.FirtName,
                LastName = reservation.Person.LastName,
                PhoneNumber = reservation.Person.Phone,
                Email = reservation.Person.Email,
                Guests = reservation.Guests,
                Comments = reservation.Comments,
                Starttime = reservation.Start,
                Duration = reservation.Duration,
                SittingId = reservation.SittingID,
                RestaurantAreaId = reservation.RestaurantAreaId,
                SittingAreaName = areas.Single(a => a.Id == reservation.RestaurantAreaId).Name,
                SittingName = sittings.Single(a => a.Id == reservation.SittingID).Name,
                SittingAreaList = new SelectList(areas, "Id", "Name", new { CurrentId = reservation.RestaurantAreaId }),
                SittingList = new SelectList(sittings, "Id", "Name")
            };   

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Edit c)
        {
            if (ModelState.IsValid)
            {
                await _reservationServices.EditReservation(c);
                await _context.SaveChangesAsync();
                TempData["AlertMessage"] = "Booking modified successfully";
            }

            return RedirectToAction("Details", "Bookings", new {area= "Customers" });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var reservation = await _reservationServices.GetReservationsByReservationId(id);
            var areas = await _restaurantServices.GetRestaurantAreaByRestaurantId(reservation.Sitting.RestaurantId);
            var sittings = await _restaurantServices.GetSittingsByRestaurantId(reservation.Sitting.RestaurantId);

            var model = new Edit
            {
                ReservationId = reservation.Id,
                RestaurantName = reservation.Sitting.Restaurant.Name,
                PersonId = reservation.Person.Id,
                FirstName = reservation.Person.FirtName,
                LastName = reservation.Person.LastName,
                PhoneNumber = reservation.Person.Phone,
                Email = reservation.Person.Email,
                Guests = reservation.Guests,
                Comments = reservation.Comments,
                Starttime = reservation.Start,
                Duration = reservation.Duration,
                SittingId = reservation.SittingID,
                RestaurantAreaId = reservation.RestaurantAreaId,
                SittingAreaName = areas.Single(a => a.Id == reservation.RestaurantAreaId).Name,
                SittingName = sittings.Single(a => a.Id == reservation.SittingID).Name,
                SittingAreaList = new SelectList(areas, "Id", "Name", new { CurrentId = reservation.RestaurantAreaId }),
                SittingList = new SelectList(sittings, "Id", "Name")
            };

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            if (ModelState.IsValid)
            {
                await _reservationServices.DeleteReservation(id);
                TempData["AlertMessage"] = "Booking deleted successfully";
            }

            return RedirectToAction("Details", "Bookings", new { area = "Customers" });
        }

        #endregion



        [HttpGet]
        public List<DateTime> ConvertDateTime(string date)
        {
            DateTime start = new();
            DateTime end = new DateTime();

            var review = DateTime.Parse(date).TimeOfDay.TotalHours;
            //breakfast
            if (review < 11) { start = DateTime.Parse(date).Date.AddHours(7); end = start.AddHours(4); }
            //lunch
            else if (review >= 11 && review < 17) { start = DateTime.Parse(date).Date.AddHours(11).AddSeconds(1); end = start.AddHours(6).AddSeconds(-1); }
            //dinner
            else if (review >= 17 && review < 23) { start = DateTime.Parse(date).Date.AddHours(16).AddSeconds(1); end = start.AddHours(7).AddSeconds(-1); }

            return new List<DateTime> { start, end };

        }

        //change this to return List of Date Time including start and end date
        [HttpGet]
        public async Task<List<Sitting>> GetAvailableSittings(int Id, string begin, string final)
        {
            DateTime? start = DateTime.Parse(begin);
            DateTime? end = DateTime.Parse(final);

            //add sitting services and add a function for this... 
            var sitting = await _context.Sittings
                .Include(r => r.Restaurant)
                .Where(r => r.RestaurantId == Id)
                .Where(s => s.Start >= start && s.End <= end)
                .Include(r => r.Reservations)
                .Where(r => r.End >= start)
                .ToListAsync();

            return sitting;
        }

        [HttpGet]
        public async Task<List<Reservation>> GetReservations(int Id, string begin, int duration)
        {
            DateTime? start = DateTime.Parse(begin);
            DateTime? end = DateTime.Parse(begin).AddMinutes(duration);

            //add this to reservation services and add a function for this... 
            var bookings = await _context.Reservations
                .Include(s => s.Sitting)
                .ThenInclude(r => r.Restaurant)
                .Where(r => r.Sitting.RestaurantId == Id)
                .Where(r => r.Start >= start && r.Start <= end)
                .ToListAsync();

            return bookings;
        }
    }
}
