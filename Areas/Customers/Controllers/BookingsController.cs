using Group_BeanBooking.Areas.Identity.Data;
using Group_BeanBooking.Data;
using Group_BeanBooking.Data.Validations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ReservationSystem.Data;

namespace Group_BeanBooking.Areas.Customers.Controllers
{

    public class BookingsController : CustomersAreaController
    {
        private readonly Queries _queries;
        private readonly Group_BeanBooking.Data.Validations.Data _data;
        public BookingsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> rolesManager) : base(context, userManager, rolesManager)
        {
            _queries = new Queries(context);
            _data = new Data.Validations.Data(context, userManager,rolesManager);
            
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

            var c = new Group_BeanBooking.Areas.Customers.Models.Bookings.Create()
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
                var reservation = new Reservation
                {
                    Start = c.Starttime,
                    Duration = c.Duration,
                    PersonId = person.Id,
                    SittingID = c.SittingId,
                    ReservationStatusID = 1,
                    ResevationOrigin = _queries.GetResevationOrigins("online"),
                    Guests = c.Guests,
                };

                _context.Reservations.Add(reservation);
                _context.SaveChanges();
            }

            return View(c);
        }


    }
}
