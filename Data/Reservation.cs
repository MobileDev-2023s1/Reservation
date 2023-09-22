namespace Group_BeanBooking.Data
{
    public class Reservation
    {
        public int Id { get; set; }
      
        public DateTime Start { get; set; }
        
        public int Duration { get; set; }
        public DateTime End { get => Start.AddMinutes(Duration); }

        public List<RestaurantTable> RestaurantTables { get; set; } = new();

        public int Guests { get; set; }


        public int SittingID { get; set; }
        public Sitting Sitting { get; set; }

        //Tie the reservation data to the user -- https://learn.microsoft.com/en-us/aspnet/core/security/authorization/secure-data?view=aspnetcore-7.0
        public int PersonId { get; set; }
        public Person Person { get; set; }

        public int RestaurantAreaId { get; set; }

        public RestaurantArea RestaurantArea { get; set; }


        public int ReservationStatusID { get; set; }
        public ReservationStatus ReservationStatus { get; set; }
        
        
        //public int ReservationOriginId { get; set; }
        public ResevationOrigin ResevationOrigin { get; set; }

        public string? Comments { get; set; }
    }
}
