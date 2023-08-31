using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Group_BeanBooking.Areas.Customers.Models.Bookings
{
    public class Create
    {
        public int Id { get; set; }

        //Customer information
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        [Required] public string PhoneNumber { get; set; }
        [Required] public string Email { get; set; }


        //Booking information

        public DateTime Starttime { get; set; }
        public int Duration { get; set; }

        public int SittingId { get; set; }
        public SelectList SittingList { get; set; }



        




    }
}
