using Microsoft.AspNetCore.Authorization;
using ReservationSystem.Data;

namespace Group_BeanBooking.Areas.Customers.Models.Bookings
{
    
    public class Details
    {
        public string Email { get; set; }
        public List<Reservation> Reservations { get; set; } = new();
        public Person Person { get; set; } = new();
    }
}
