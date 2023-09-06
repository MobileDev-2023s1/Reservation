namespace ReservationSystem.Data
{
    public class Person
    {
        public int Id { get; set; }
        public string  FirtName { get; set; }
        public string LastName { get; set; }
        public string  Email { get; set; }
        public string  Phone { get; set; }
        public List<Reservation> Reservations { get; set; } = new();




    }
}
