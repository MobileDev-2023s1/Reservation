﻿using Group_BeanBooking.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Windows.Markup;

namespace Group_BeanBooking.Areas.Customers.Models.Bookings
{
    public class Edit : Create
    {
        [ValidateNever] public int PersonId { get; set; }

        [ValidateNever] public string SittingAreaName { get; set; }

        [ValidateNever] public string SittingName { get; set; }

        [ValidateNever] public int ReservationStatusId { get; set; }

        [ValidateNever] public List<RestaurantTable> RestaurantTables { get; set;}


    }
}
