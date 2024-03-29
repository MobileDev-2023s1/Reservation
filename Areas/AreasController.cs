﻿using Group_BeanBooking.Areas.Identity.Data;
using Group_BeanBooking.Data;
using Group_BeanBooking.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Group_BeanBooking.Data;

namespace Group_BeanBooking.Areas
{
    public class AreasController : Controller
    {
        protected readonly ApplicationDbContext _context;
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly RoleManager<IdentityRole> _rolesManager;
        

        public AreasController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> rolesManager)
        {
            _context = context;
            _userManager = userManager;
            _rolesManager = rolesManager;
            
        }
        
    }
}
