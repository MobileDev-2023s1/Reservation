using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Group_BeanBooking.Areas.Identity.Data;

// Add profile data for application users by adding properties to the User class
public class ApplicationUser : IdentityUser
{
    [PersonalData]
    public string FirstName { get; set; }
    [PersonalData]
    public string LastName { get; set; }

    [PersonalData]
    public string PhoneNumber { get; set; }


}

