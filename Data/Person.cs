using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Group_BeanBooking.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace Group_BeanBooking.Data
{
    public class Person
    {
        [Key]
        public int Id { get; set; }
        public string  FirtName { get; set; }
        public string LastName { get; set; }
        public string  Email { get; set; }
        public string  Phone { get; set; }
        public List<Reservation> Reservations { get; set; } = new();

        [ForeignKey("UserId")]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }



    }
}
