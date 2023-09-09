
using Group_BeanBooking.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto;
using Group_BeanBooking.Data;

namespace Group_BeanBooking.Areas.Administration.Controllers
{
    [Area("Administration"), Authorize(Roles = "Administrator")]
    public class AdministrationAreaController : Controller
    {
        protected readonly ApplicationDbContext _context;
        


        public AdministrationAreaController(ApplicationDbContext context)
        {
            _context = context;
          

        }

    }
}
