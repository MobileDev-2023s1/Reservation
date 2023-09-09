
using Group_BeanBooking.Areas.Administration.Controllers;
using Group_BeanBooking.Data;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto;
using ReservationSystem.Data;

namespace Group_BeanBooking.Areas.Administration.Controllers
{
    public class HomeController : AdministrationAreaController
    {
        public HomeController(ApplicationDbContext context) : base(context)
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}