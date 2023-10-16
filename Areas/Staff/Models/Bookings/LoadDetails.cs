using Group_BeanBooking.Areas.Customers.Models.Bookings;
using Group_BeanBooking.Data;

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

using Org.BouncyCastle.Bcpg;

namespace Group_BeanBooking.Areas.Staff.Models.Bookings
{
    public class LoadDetails
    {
        public int RestaurantId { get; set; }
        public SelectList RestaurantList { get; set; }

        public string CustomerEmail { get; set; }
        public string StatusId { get; set; }
        public SelectList StatusesList { get; set;}

        public int RestaurantAreaId { get; set; }
        public SelectList SittingAreaList { get; set; }

        public int SittingId { get; set; }
        public SelectList SittingList { get; set; }

        public Create UpdateDails { get; set; }


    }
}
