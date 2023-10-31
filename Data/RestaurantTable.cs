namespace Group_BeanBooking.Data
{
    public class RestaurantTable
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int RestaurantAreaId { get; set; }
        public List<Reservation> Reservations { get; set; } = new();

    }
}
