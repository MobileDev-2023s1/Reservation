using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Group_BeanBooking.Areas.Customers.Models.Bookings
{
    public class Create
    {
        public int Id { get; set; }

        [ValidateNever] public string RestaurantName { get; set; }

        //Customer information
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        [Required] public string PhoneNumber { get; set; }
        [Required] public string Email { get; set; }

        [Required] public int Guests { get; set; }

        [Required] public string Comments { get; set; }


        //Booking information

        [ValidateNever] public DateTime Starttime { get; set; }
        [ValidateNever] public int Duration { get; set; }

        [ValidateNever] public int SittingAreaId { get; set; }
        [ValidateNever] public SelectList SittingAreaList { get; set; }

        [ValidateNever] public int SittingId { get; set; }
        [ValidateNever] public SelectList SittingList { get; set; }





        




    }
}
