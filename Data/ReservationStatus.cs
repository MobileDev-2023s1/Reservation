 namespace Group_BeanBooking.Data
{
    public class ReservationStatus
    {
        public int Id { get; set; }
        public string  Name { get; set; }
        public List<Reservation> Reservations { get; set; } = new();

    }
}
