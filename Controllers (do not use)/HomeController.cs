using Group_BeanBooking.Areas.Identity.Data;
using Group_BeanBooking.Data;
using Group_BeanBooking.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Group_BeanBooking.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SeedData _seedData;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _rolesManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context , 
            UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> rolesManager)
        {
            _logger = logger;
            _context = context;
            _seedData = new SeedData(context, userManager, rolesManager);
            
        }

        public IActionResult Index()
        {
            _seedData.SeedDataMain();
            return View();
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
    }
}