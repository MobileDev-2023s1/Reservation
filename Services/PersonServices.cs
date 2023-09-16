using Group_BeanBooking.Areas;
using Group_BeanBooking.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Group_BeanBooking.Data;
using Group_BeanBooking.Areas.Customers.Models.Bookings;
using Group_BeanBooking.Data;
using Microsoft.EntityFrameworkCore;

namespace Group_BeanBooking.Services
{
    public class PersonServices : ServicesArea
    {
        public PersonServices(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> rolesManager) : base(context, userManager, rolesManager)
        {
           
        }

        /// <summary>
        /// Validates whether the values entered for reservation exist or not in the data base
        /// either as a Registered user or as a not registered Person
        /// </summary>
        /// <param name="c">It is the information included in the form for creating a reservation </param>
        /// <returns>The information from the user either logged in or not </returns>
        public async Task<Person> UserValidation(Create c)
        {
            var userByID = await GetUserById(c.UserId);
            var userByEmail = await GetUserByEmail(c.Email);
            var personByEmail = await GetPersonByEmail(c.Email);
            var personById = await GetPersonById(c.UserId);

            //means not logged in
            if (c.UserId == null)
            {
                //if they exist in both tables then return the Person
                if (userByEmail != null && personByEmail != null)
                {
                    var person = new Person
                    {
                        FirtName = c.FirstName,
                        LastName = c.LastName,
                        Email = c.Email,
                        Phone = c.PhoneNumber,
                        Id = personByEmail.Id
                    };

                    return person;
                }
                //If email exist in DB but not in people
                else if (userByEmail != null && personByEmail == null)
                {
                    return CreatePerson(null, userByEmail);
                }
                //exists in people and not in DB
                else if (userByEmail == null && personByEmail != null)
                {
                    return personByEmail;
                }
                //doesn't exist in either
                else
                {
                    return CreatePerson(c, null);
                }

            } 
            //mean person has logged in
            else
            {
                //logged in and does not exist in person list
                if (personById == null)
                {
                    return CreatePerson(null, userByID);
                }
                //logged in and exist in the person list
                else
                {
                    return personById;
                }
            }
        }

        //if c has something in it, means person has logged in.
        //Otherwise, User 
        public Person CreatePerson(Create? c, ApplicationUser? user)
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

        #region Includes Queries in the DB Person Related 

        public async Task<Person> GetPersonById(string? UserId)
        {
            if (UserId == null)
            {
                return null;
            }
            {
                return await _context.People.FirstOrDefaultAsync(p => p.UserId.Equals(UserId));
            }
        }

        public async Task<Person> GetPersonByEmail(string email)
        {
            return await _context.People.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<ApplicationUser> GetUserById(string id)
        {
            return await _context.Users.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<ApplicationUser> GetUserByEmail(string? email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        #endregion

    }
}
