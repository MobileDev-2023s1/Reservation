namespace Group_BeanBooking.Areas.Staff.Models.Bookings
{
    public class TableStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }

        public int ReservationId { get; set; }
    }
}
