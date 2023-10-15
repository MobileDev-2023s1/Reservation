using System.Linq.Expressions;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography.X509Certificates;

using Google.Protobuf.Collections;

using Group_BeanBooking.Data;
using Humanizer;
using Microsoft.EntityFrameworkCore.Internal;
using NuGet.Packaging;

namespace Group_BeanBooking.Areas.Staff.Data
{
    public class WhereClause
    {
        public string Email { get; set; }
        public int RestaurantId { get; set; }
        public int StatusId { get; set; }
        public int BookingId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }


        public Expression<Func<Reservation, bool>> BuildWhereClause(WhereClause clause)
        {
            /*
             * Expression<Func<Reservation, bool>>: This is a data type declaration. It specifies that whereClause is a variable 
             * that will hold an expression. The Expression<TDelegate> is a class in the System.Linq.Expressions namespace used 
             * for building expression trees, which are representations of code as data. In this case, it's specifying that the 
             * expression is a function (lambda) that takes a Reservation as input and returns a bool result.
             */

            Expression<Func<Reservation, bool>> whereClause = null;
            Expression<Func<Reservation, bool>> email = clause.Email != null ? r => r.Person.Email == clause.Email : null; 
            Expression<Func<Reservation, bool>> restaurantId = clause.RestaurantId != 0 ? r => r.Sitting.RestaurantId == clause.RestaurantId : null;
            Expression<Func<Reservation, bool>> statusId = clause.StatusId != 0 ? r => r.ReservationStatusID == clause.StatusId : null;
            Expression<Func<Reservation, bool>> bookingId = clause.BookingId != 0 ? r => r.Id == clause.BookingId : null;

            if (clause.Email != null && restaurantId != null && statusId != null)
            {
                whereClause = r => r.Person.Email == clause.Email && r.Sitting.RestaurantId == clause.RestaurantId && r.ReservationStatusID == clause.StatusId;
            } else if(restaurantId != null && statusId != null)
            {
                whereClause = r => r.Sitting.RestaurantId == clause.RestaurantId && r.ReservationStatusID == clause.StatusId;
            } else if(email != null && statusId != null)
            {
                whereClause = r => r.Person.Email == clause.Email && r.ReservationStatusID == clause.StatusId;
            }else if(email != null && restaurantId != null)
            {
                whereClause = r => r.Person.Email == clause.Email && r.Sitting.RestaurantId == clause.RestaurantId;
            } else if(statusId != null)
            {
                whereClause = r => r.ReservationStatusID == clause.StatusId;
            }else if( restaurantId != null)
            {
                whereClause = r => r.Sitting.RestaurantId == clause.RestaurantId;
            }
            else if (email != null)
            {
                whereClause = r => r.Person.Email == clause.Email;
            } 

            return whereClause;
        }

       
    }
}
