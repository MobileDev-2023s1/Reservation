using Group_BeanBooking.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Group_BeanBooking.Data;

namespace Group_BeanBooking.Areas.Customers.Models.Bookings
{
    
    public class Details
    {
        public string Email { get; set; }
        public List<Reservation> Reservations { get; set; } = new();
        public Person Person { get; set; } = new();
    }
}
