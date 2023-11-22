using System.Data.Entity;

using Group_BeanBooking.Areas.Identity.Data;
using Group_BeanBooking.Services;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Group_BeanBooking.Data
{
    public class ToTest
    {
 

        public int Days(DateTime date)
        {
            switch (date.Month)
            {
                case 1: case 3: case 5: case 7: case 8: case 10: case 12: return 31; break;
                case 4: case 6: case 9: case 11: return 30; break;
                case 2:
                if (DateTime.IsLeapYear(date.Year))
                {
                    return 29;
                }
                else
                {
                    return 28;
                }
                break;
            }

            return 0;
        }



        public List<DateTime> ConvertDateTime(string date)
        {
            DateTime start = new();
            DateTime end = new DateTime();

            var review = DateTime.Parse(date).TimeOfDay.TotalHours;
            //breakfast
            if (review < 11) { start = DateTime.Parse(date).Date.AddHours(7); end = start.AddHours(4); }
            //lunch
            else if (review >= 11 && review < 17) { start = DateTime.Parse(date).Date.AddHours(11).AddSeconds(1); end = start.AddHours(6).AddSeconds(-1); }
            //dinner
            else if (review >= 17 && review < 23) { start = DateTime.Parse(date).Date.AddHours(16).AddSeconds(1); end = start.AddHours(7).AddSeconds(-1); }

            return new List<DateTime> { start, end }; 
        }

        public Person GetPersonByEmail(string email)
        {
            try
            {
                var list = new List<Person>
                {
                    new Person { FirtName="Diego", LastName="Calvo Amaya", Email="dc@g.com", Phone="12234544646", UserId="34cc146a-e556-44cf-a00e-62e6587e0170"},
                    new Person { FirtName="Liliana", LastName="Euse Rincon", Email="lm@g.com", Phone="5698456999", UserId="5d36e135-0dd4-4ddd-bd22-3b0cdabce11e"},
                    new Person { FirtName="Andres", LastName="Quito", Email="eq@g.com", Phone="5698456999", UserId="9c467863-4ae6-4f5f-b4a9-16beb5f8ef0d"},
                };
                
                return list.FirstOrDefault(p=> p.Email == email);

            }
            catch (Exception ex)
            {
                return null;
            }
        }




    }


}