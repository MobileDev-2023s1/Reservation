using System.Data.Entity;
using System.Diagnostics.Metrics;

using Group_BeanBooking.Areas.Identity.Data;
using Group_BeanBooking.Areas.Staff.Data;
using Group_BeanBooking.Data;
using Group_BeanBooking.Services;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Group_BeanBooking.Areas.Staff.Controllers
{
    public class TablesController : StaffAreaController
    {
        private readonly RestaurantServices _restaurantServices;

        public TablesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> rolesManager)
            : base(context, userManager, rolesManager)
        {
            _restaurantServices = new RestaurantServices(context, userManager, rolesManager);
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetListofTables(int areaId)
        {
            if(ModelState.IsValid)
            {
              
                var list = await _restaurantServices.GetListofTables(areaId);
                
                if(list == null)
                {
                    return Ok("nothing found");
                }else
                {
                    return Ok(list);
                }
            }
            else
            {
                return Ok("review");
            }
            
        }
    }
}
