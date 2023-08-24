using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ReservationSystem.Data
{
    public class RestaurantArea
    {
        public int Id { get; set; }
        public String  Name { get; set; }
        public List<RestaurantTable> restaurantTables { get; set; } = new();

    }
}
 