namespace Group_BeanBooking.Data
{
    public class RestaurantTable
    {
        public int Id { get; set; }
        public String Name { get; set; }

        public int RestaurantAreaId { get; set; }
        public List<Reservation> reservations { get; set; } = new();

    }
}
