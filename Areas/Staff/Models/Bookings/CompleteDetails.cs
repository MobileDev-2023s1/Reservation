using Google.Protobuf.WellKnownTypes;

using Group_BeanBooking.Data;

namespace Group_BeanBooking.Areas.Staff.Models.Bookings
{
    public class CompleteDetails
    {
        public int ReservationId { get; set; }
        public int RestaurantID { get; set; }

        public int Duration { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get => StartTime.AddMinutes(Duration); }

        public int SittingId { get; set; }
        public int PersonId { get; set; }

        public Person Person { get; set; }= new Person();

        public int RestaurantAreaId { get; set; }
        public ReservationStatus ReservationStatus { get; set; } = new ReservationStatus();

        public string Comments { get; set; }


    }
}
