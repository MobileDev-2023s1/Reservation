using Group_BeanBooking.Areas.Customers.Data;
using Group_BeanBooking.Areas.Identity.Data;
using Group_BeanBooking.Data;
using Group_BeanBooking.Data.Validations;
using Group_BeanBooking.Areas.Customers.Models.Bookings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ReservationSystem.Data;
using System.Linq.Expressions;

namespace Group_BeanBooking.Areas.Customers.Controllers
{

    public class BookingsController : CustomersAreaController
    {
        private readonly Queries _queries;
        private readonly Group_BeanBooking.Data.Validations.ValidateData _data;
        public BookingsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> rolesManager) : base(context, userManager, rolesManager)
        {
            _queries = new Queries(context);
            _data = new ValidateData(context, userManager,rolesManager);
            
        }
        //data that
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create(int id)
        {
            var restInfo = _queries.GetRestaurantByID(id);
            var areas = _queries.GetRestaurantAreaByRestaurantId(id);
            var sittings = _queries.GetSittingsByRestaurantId(id);

            var c = new Create()
            {
                RestaurantName = restInfo.Name,
                SittingAreaList = new SelectList(areas, "Id", "Name"),
                SittingList = new SelectList(sittings, "Id", "Name"),
            };
            
            return View(c);
        }

        [HttpPost]
        public IActionResult Create(Group_BeanBooking.Areas.Customers.Models.Bookings.Create c)
        {
            var person = _data.UserValidation(c);

            if(ModelState.IsValid)
            {
                new Bookings(_context).CreateReservation(person, c);
            }

            return RedirectToAction("Details", "Bookings" , new  { id = person.Id , area="Customers"});
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var model = new Group_BeanBooking.Areas.Customers.Models.Bookings.Details();
            if(id != 0)
            {
                model.Reservations = _queries.GetReservations(id);

            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Details(string? email , int? id) 
        {
            var model = new Group_BeanBooking.Areas.Customers.Models.Bookings.Details();
            Person user = new();
            List<Reservation> bookings = new();

            

            if (ModelState.IsValid)
            {
                //person has not logged in and then email is used for search
                if(id == null)
                {
                    user = _queries.GetPersonByEmail(email);
                    bookings = _queries.GetReservationsByPersonId(user.Id);

                }
                else
                {
                    bookings = _queries.GetReservationsByPersonId(id);
                }

            }           

            model.Reservations = bookings;
            model.Person = user;
            model.Email = email;
            
            return View(model);
        }




    }
}
