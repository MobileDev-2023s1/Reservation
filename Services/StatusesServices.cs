using Group_BeanBooking.Areas.Identity.Data;
using Group_BeanBooking.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Group_BeanBooking.Services
{
    public class StatusesServices : ServicesArea
    {
        public StatusesServices(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> rolesManager) : base(context, userManager, rolesManager)
        {

        }

        public async Task<List<ReservationStatus>> GetListReservationStatus()
        {
            return await _context.ReservationStatuses
                //.Where(r=> )
                .ToListAsync();
        }
    }
}
