namespace ReservationSystem.Data
{
    public class Restaurant
    {
        public int Id { get; set; }
        public string Name  { get; set; }
        public String Phone { get; set; }
      
        public List<Sitting> Sittings{ get; set; }=new();
        public List<RestaurantArea> RestaurantAreas { get; set; } = new();

    }
}
