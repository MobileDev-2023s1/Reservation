using System.Linq.Expressions;
using Group_BeanBooking.Data;

using Humanizer;

using LinqKit;



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

        public int RestaurantAreaId { get; set; }

        public int Duration { get; set; }

        //public WhereClauseCalendarView(int bookingId)
        //{
        //    BookingId = bookingId;
        //}


        public Expression<Func<Reservation, bool>> BuildCalendarViewReservationClause(WhereClause clause)
        {
            /*
             * Expression<Func<Reservation, bool>>: This is a data type declaration. It specifies that whereClause is a variable 
             * that will hold an expression. The Expression<TDelegate> is a class in the System.Linq.Expressions namespace used 
             * for building expression trees, which are representations of code as data. In this case, it's specifying that the 
             * expression is a function (lambda) that takes a Reservation as input and returns a bool result.
             */

            var whereClause = PredicateBuilder.New<Reservation>(true);

            Expression<Func<Reservation, bool>> email = clause.Email != null ? r => r.Person.Email == clause.Email : null;
            Expression<Func<Reservation, bool>> restaurantId = clause.RestaurantId != 0 ? r => r.Sitting.RestaurantId == clause.RestaurantId : null;
            Expression<Func<Reservation, bool>> statusId = clause.StatusId != 0 ? r => r.ReservationStatusID == clause.StatusId : null;
            Expression<Func<Reservation, bool>> bookingId = clause.BookingId != 0 ? r => r.Id == clause.BookingId : null;
            Expression<Func<Reservation, bool>> startDate = clause.StartDate.ToString() != "1/01/0001 12:00:00 AM" ? r => r.Start >= clause.StartDate : null;
            Expression<Func<Reservation, bool>> endDate = clause.EndDate.ToString() != "1/01/0001 12:00:00 AM" ? r => r.Start <= clause.EndDate : null;
            

            if (email != null)
            {
                whereClause = whereClause.And(email);
            }
            if (restaurantId != null)
            {
                whereClause = whereClause.And(restaurantId);
            }
            if (statusId != null)
            {
                whereClause = whereClause.And(statusId);
            }
            if (bookingId != null)
            {
                whereClause = whereClause.And(bookingId);
            }
            if (startDate != null)
            {
                whereClause = whereClause.And(startDate);
            }
            if (endDate != null)
            {
                whereClause = whereClause.And(endDate);
            }
            
            return whereClause;

            #region previous code working for reservations

            #endregion 
        }

        public Expression<Func<Reservation,bool>> BuildListOfBookings(Reservation clause)
        {
            var whereClause = PredicateBuilder.New<Reservation>(true);
            Expression<Func<Reservation, bool>> restaurantAreaId = clause.RestaurantAreaId != null ? r => r.RestaurantAreaId == clause.RestaurantAreaId : null;
            Expression<Func<Reservation, bool>> startTime = clause.Start.ToString() != "1/01/0001 12:00:00 AM" ? r => r.Start <= clause.Start : null;
            Expression<Func<Reservation, bool>> endTime = clause.End.ToString() != "1/01/0001 12:00:00 AM" ? r => r.Start.AddMinutes(clause.Duration) >= clause.Start : null;
            Expression<Func<Reservation, bool>> statusId = clause.ReservationStatusID != 0 ? r => r.ReservationStatusID != 3 && r.ReservationStatusID != 5 : null;
            

            if (restaurantAreaId != null)
            {
                whereClause = whereClause.And(restaurantAreaId);
            }

            if (startTime != null)
            {
                whereClause = whereClause.And(startTime);
            }
            if (endTime != null)
            {
                whereClause = whereClause.And(endTime);
            }
            //if (statusId != null)
            //{
            //    whereClause = whereClause.And(statusId);
            //}

            return whereClause;
        }

        public Expression<Func<RestaurantTable, bool>> BuildRestaurantTableClause(RestaurantTable table)
        {
            var whereClause = PredicateBuilder.New<RestaurantTable>(true);
            Expression<Func<RestaurantTable,bool>> restaurantAreaId = table.RestaurantAreaId > 0 ? 
                t=> t.RestaurantAreaId == table.RestaurantAreaId : null;
            Expression<Func<RestaurantTable, bool>> tableId = table.Id > 0 ? t=> t.Id == table.Id : null;

            if(restaurantAreaId!= null)
            {
                whereClause = whereClause.And(restaurantAreaId);
            }
            if (tableId!= null)
            {
                whereClause = whereClause.And(tableId);
            }

            return whereClause;

        }

    }
}
