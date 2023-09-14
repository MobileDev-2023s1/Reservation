using Group_BeanBooking.Areas.Identity.Data;
using Group_BeanBooking.Data;
using Group_BeanBooking.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ReservationSystem.Data;
using System.Diagnostics;

namespace Group_BeanBooking.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        protected readonly ILogger<HomeController> _logger;
        protected readonly SeedData _seedData;
        protected readonly ApplicationDbContext _context;
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly RoleManager<IdentityRole> _rolesManager;
        private readonly Queries _queries;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context , 
            UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> rolesManager)
        {
            _logger = logger;
            _context = context;
            _seedData = new SeedData(context, userManager, rolesManager);
            _queries = new Queries(context);

        }

        [HttpGet]
        public IActionResult Index()
        {
            //_seedData.SeedDataMain();
            var c = new RestaurantList();
            c.RestList = new SelectList(_context.Restaurants.ToList(), "Id", "Name");
            var list = _context.Restaurants.ToList();
            c.Restaurants.AddRange(list);
            return View(c);
        }

        [HttpPost]
        public IActionResult Index(RestaurantList c)
        {
            return RedirectToAction("Create", "Bookings", new { id = c.RestaurantId , area = "Customers"});
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult RedirectUser()
        {
            //var roles = _rolesManager.Roles.ToList();

            if (User.IsInRole("Customer"))
            {
                var user = _queries.GetPersonByEmail(User.Identity.Name);

                return RedirectToAction("Details", "Bookings", new { id = user == null? 0 : user.Id , area = "Customers" });
            } else
            {
                return View();
            }
            

        }
    }
}