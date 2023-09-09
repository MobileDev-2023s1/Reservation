using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Group_BeanBooking.Data
{
    public class RestaurantArea
    {
        public int Id { get; set; }
        public String  Name { get; set; }

        public int RestaurantId { get; set; }
        
        public List<RestaurantTable> restaurantTables { get; set; } = new();

    }
}
 