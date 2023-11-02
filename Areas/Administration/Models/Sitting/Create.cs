using Microsoft.AspNetCore.Mvc.Rendering;
using Group_BeanBooking.Data;

namespace Group_BeanBooking.Areas.Administration.Models.Sitting
{
    public class Create
    {
      
        public string Name { get; set; }


        public bool Closed { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public TimeOnly StartTime { get; set; }

        public int DurationH { get; set; }
        public int DurationM { get; set; }


        public int Capacity { get; set; }

        public int TypeId { get; set; }
        public SelectList? SittingTypes { get; set; }
      
        public int Interval { get; set; }
        public double Repeats { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }

    }
}
