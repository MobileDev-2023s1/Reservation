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

        public int Capacity { get; set; }

        public int TypeId { get; set; }
        public SelectList? SittingTypes { get; set; }
       
    }
}
