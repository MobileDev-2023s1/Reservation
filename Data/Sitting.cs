namespace Group_BeanBooking.Data
{
    public class Sitting
    {
        public int Id { get; set; }
        public string Name { get; set; }    
      

        public bool Closed { get; set; }
        public DateTime Start {get; set; }
        public DateTime End { get; set; }

        public int Capacity { get; set; }

        public int RestaurantId{ get; set; }
        public  Restaurant Restaurant { get; set; }
        public int TypeId { get; set; }
        public SittingType Type { get; set; }
        public List<Reservation> Reservations { get; set; } = new();
        public Guid? Guid { get; set; }


        public string RepeatPattern { get; set; }
        public int Interval  { get; set; }
        public int Repeats { get; set; }




    }
}
