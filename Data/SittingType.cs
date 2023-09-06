namespace ReservationSystem.Data
{
    public class SittingType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Sitting> Sittings { get; set; } = new();
      


    }
}
