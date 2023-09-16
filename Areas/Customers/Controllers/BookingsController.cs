
using Group_BeanBooking.Areas.Identity.Data;
using Group_BeanBooking.Data;
using Group_BeanBooking.Areas.Customers.Models.Bookings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Group_BeanBooking.Data;
using System.Linq.Expressions;
using Group_BeanBooking.Services;

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

        [HttpGet]
        public async Task<IActionResult> Create(int id)
        {
            var restInfo = await _restaurantServices.GetRestaurantByID(id);
            var areas = await _restaurantServices.GetRestaurantAreaByRestaurantId(id);
            var sittings = await _restaurantServices.GetSittingsByRestaurantId(id);

            var c = new Create()
            {
                RestaurantName = restInfo.Name,
                SittingAreaList = new SelectList(areas, "Id", "Name"),
                SittingList = new SelectList(sittings, "Id", "Name"),
            };
            
            return View(c);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Group_BeanBooking.Areas.Customers.Models.Bookings.Create c)
        {
            var person = await _personServices.UserValidation(c);

            if(ModelState.IsValid)
            {
                await _reservationServices.CreateReservation(person, c);
            }

            return RedirectToAction("Details", "Bookings" , new  { id = person.Id , area="Customers"});
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var model = new Group_BeanBooking.Areas.Customers.Models.Bookings.Details();
            if(id != 0)
            {
                model.Reservations = await _reservationServices.GetReservations(id);
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
                {
                    await _personServices.GetPersonByEmail(email);
                    bookings = await _reservationServices.GetReservationsByPersonId(user.Id);
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

    }
}
