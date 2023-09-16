using Group_BeanBooking.Areas.Customers.Models.Bookings;
using Group_BeanBooking.Areas.Identity.Data;
using Group_BeanBooking.Data;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Group_BeanBooking.Services
{
    public class PersonServices : ServicesArea
    {
        public PersonServices(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> rolesManager) : base(context, userManager, rolesManager)
        {
           
        }

        #region Includes Queries in the DB Person Related 

        public async Task<Person> GetPersonById(string? UserId)
        {
            return await _context.People.SingleOrDefaultAsync(p => p.UserId.Equals(UserId));
           
        }

        public async Task<Person> GetPersonByEmail(string email)
        {
            try
            {
                return await _context.People.SingleOrDefaultAsync(p => p.Email == email);

            }catch(Exception ex)
            {
                return null;
            }
        }

        public async Task<ApplicationUser> GetUserById(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<ApplicationUser> GetUserByEmail(string? email)
        {
            var result = await _userManager.FindByEmailAsync(email);
            return result;
        }

        #endregion

        /// <summary>
        /// Validates whether the values entered for reservation exist or not in the data base
        /// either as a Registered user or as a not registered Person
        /// </summary>
        /// <param name="c">It is the information included in the form for creating a reservation </param>
        /// <returns>The information from the user either logged in or not </returns>
        public async Task<Person> UserValidation(Create? c, ApplicationUser? user)
        {
            //var userByID = c == null ? await GetUserById(user.Id) : await GetUserById(c.UserId);
            var userByEmail = c == null ? await GetUserByEmail(user.Email) : await GetUserByEmail(c.Email);
            var personByEmail = c == null ? await GetPersonByEmail(user.Email) : await GetPersonByEmail(c.Email);
            var personById = c  == null ? await GetPersonById(user.Id) : await GetPersonById(c.UserId);

                //if they exist in both tables then return the Person
                if (userByEmail != null && personByEmail != null)
                {
                    //is person by ID present? Means that user has Id in the person table
                    if(personById != null)
                    {
                        return personById;

                    } 
                    else //if User Id not present then asign it and then return person
                    {
                        await _context.People.ExecuteUpdateAsync(b => b.SetProperty(b => b.UserId, userByEmail.Id));
                        return personByEmail;
                    }
                }
                //If email exist in DB but not in people
                else if (userByEmail != null && personByEmail == null)
                {
                    return await CreatePerson(null, userByEmail);
                }
                //exists in people and not in DB
                else if (userByEmail == null && personByEmail != null)
                {
                    return personByEmail;
                }
                //doesn't exist in either
                else
                {
                    return await CreatePerson(c, null);
                }
        }

        //if c has something in it, means person has logged in.
        //Otherwise, User 
        public async Task<Person> CreatePerson(Create? c, ApplicationUser? user)
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

            await _context.People.AddAsync(person);
            await _context.SaveChangesAsync();

            return person;
        }

        

    }
}
