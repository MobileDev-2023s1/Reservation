using Group_BeanBooking.Areas.Customers.Data;
using Group_BeanBooking.Areas.Identity.Data;
using Group_BeanBooking.Data;
using Group_BeanBooking.Data.Validations;
using Group_BeanBooking.Areas.Customers.Models.Bookings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ReservationSystem.Data;

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

            return RedirectToAction("Details", "Bookings" , new  { id = c.Id , area="Customers"});
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var list = _queries.GetReservationsByPersonId(id);

            return View();
        }

        [HttpPost]
        public IActionResult Details(string email) 
        {
            var model = new Group_BeanBooking.Areas.Customers.Models.Bookings.Details();
            var user = _queries.GetPersonByEmail(email);
            var bookings = _queries.GetReservations(user.Id);

            model.Reservations = bookings;
            model.Person = user;
            model.Email = email;

            
            return View(model);
        }


    }
}
