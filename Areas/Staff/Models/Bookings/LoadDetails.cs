using Group_BeanBooking.Data;

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


    }
}
