using Group_BeanBooking.Areas.Customers.Models.Bookings;
using Group_BeanBooking.Data;

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

using Org.BouncyCastle.Bcpg;

namespace Group_BeanBooking.Areas.Staff.Models.Bookings
{
    public class LoadDetails : CompleteDetails
    {
        public string CustomerEmail { get; set; }
        
        public SelectList RestaurantList { get; set; }

        public SelectList StatusesList { get; set;}

       public SelectList NewReservationStatusList { get; set; }

        public int NewReservationStatusId { get; set; } 

        //public CompleteDetails UpdateDails { get; set; }


    }
}
