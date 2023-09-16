using Group_BeanBooking.Areas.Identity.Data;
using Group_BeanBooking.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Group_BeanBooking.Areas.Customers.Models.Bookings
{
    public class Create
    {
        public int ReservationId { get; set; }

        [ValidateNever] public string RestaurantName { get; set; }

        [ValidateNever] public string UserId { get; set; }

        //Customer information
        [ValidateNever] public string FirstName { get; set; }
        [ValidateNever] public string LastName { get; set; }
        [ValidateNever] public string PhoneNumber { get; set; }
        [ValidateNever] public string Email { get; set; }

        [ValidateNever] public int Guests { get; set; }

        [ValidateNever] public string? Comments { get; set; }


        //Booking information

        [Required] public DateTime Starttime { get; set; }
        [Required] public int Duration { get; set; }

        [Required] public int RestaurantAreaId { get; set; }
        [ValidateNever] public SelectList SittingAreaList { get; set; }

        [Required] public int SittingId { get; set; }
        [ValidateNever] public SelectList SittingList { get; set; }







        




    }
}
