namespace Group_BeanBooking.Areas.Administration.Models.Sitting
{
    public class Edit:Create
    {
        public int Id { get; set; }
        public Guid? Guid { get; set; }
        public string? RepeatPattern { get; set; }
        public int Interval { get; set; }
        public int Repeats { get; set; }
    }
}
