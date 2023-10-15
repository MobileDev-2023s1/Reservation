using Google.Protobuf.WellKnownTypes;

using Group_BeanBooking.Data;

namespace Group_BeanBooking.Areas.Staff.Models.Bookings
{
    public class CompleteDetails
    {
        public int BookingId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<RestaurantTable> AssignedTable { get; set; }
        public int Duration { get; set; }
        public int SittingId { get; set; }
        public int RestaurantID { get; set; }
        public string RestaurantName { get; set; }
        public int Guest { get; set; }
        public int PersonId { get; set; }
        public string PersonName { get; set; }

        public int RestaurantAreaId { get; set; }
        public int ReservationStatusId { get; set; }
        public string Comments { get; set; }


    }
}
