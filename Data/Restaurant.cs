using System.ComponentModel.DataAnnotations;

namespace Group_BeanBooking.Data
{
    public class Restaurant
    {
        [Key]
        public int Id { get; set; }
        public string Name  { get; set; }
        public string Phone { get; set; }
      
        public List<Sitting> Sittings{ get; set; }=new();
        public List<RestaurantArea> RestaurantAreas { get; set; } = new();

    }
}
