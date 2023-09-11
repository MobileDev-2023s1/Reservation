using Group_BeanBooking.Areas;
using Group_BeanBooking.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using ReservationSystem.Data;
using Group_BeanBooking.Areas.Customers.Models.Bookings;

namespace Group_BeanBooking.Data.Validations
{
    public class ValidateData : AreasController
    {
        private readonly Queries _queries;
        public ValidateData(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> rolesManager) : base(context, userManager, rolesManager)
        {
            _queries = new Queries(context);
        }

        public Person UserValidation(Create c)
        {
            var userID = _queries.GetUserById(c.UserId);
            var userEmail = _queries.GetUserByEmail(c.Email);
            var personEmail = _queries.GetPersonByEmail(c.Email);
            var personId = _queries.GetPersonById(c.UserId);

            //means not logged in
            if (c.UserId == null)
            {
                if(userEmail != null && personEmail != null)
                {
                    var person = new Person
                    {
                        FirtName = c.FirstName,
                        LastName = c.LastName,
                        Email = c.Email,
                        Phone = c.PhoneNumber,
                        Id = personEmail.Id
                        
                    };

                    return person;
                }
                //If email exist in DB but not in people
                else if(userEmail != null && personEmail == null)
                {
                    return CreatePerson(null, userEmail);
                } 
                //exists in people and not in DB
                else if(userEmail == null && personEmail != null)
                {
                    return personEmail;
                }
                //doesn't exist in either
                else
                {
                    return CreatePerson(c, null);
                }
                
            } //mean person has logged in
            else
            {
                //logged in and does not exist in person list
                if(personId == null )
                {
                    return CreatePerson(null, userID);
                }
                //logged in and exist in the person list
                else
                {
                    return personId;
                }
            }
        }           
            
        
        public Person CreatePerson(Create? c , ApplicationUser? user)
        {
            var person = new Person();
            if (c == null)
            {
                person.FirtName = user.FirstName;
                person.LastName = user.LastName;
                person.Email = user.Email;
                person.Phone = user.PhoneNumber;
                person.UserId = user.Id;
            }
            else
            {
                person.FirtName = c.FirstName;
                person.LastName = c.LastName;
                person.Email = c.Email;
                person.Phone = c.PhoneNumber;
                
            }

            _context.People.Add(person);
            _context.SaveChanges();

            return person;
        }

       
    }
}
