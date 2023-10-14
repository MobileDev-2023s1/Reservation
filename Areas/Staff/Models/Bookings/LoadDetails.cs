using Group_BeanBooking.Data;

using Org.BouncyCastle.Bcpg;

namespace Group_BeanBooking.Areas.Staff.Models.Bookings
{
    public class LoadDetails
    {
        public int BookingId { get; set; }
        public string Name { get; set; }
        private int Duration { get; set; }
        public int StatusId { get; set; }

        public DateTime Start { get; set; }
        public DateTime End { get => Start.AddMinutes(Duration); }
    }
}
