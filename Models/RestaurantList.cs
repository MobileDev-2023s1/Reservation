﻿using Microsoft.AspNetCore.Mvc.Rendering;
using ReservationSystem.Data;

namespace Group_BeanBooking.Models
{
    public class RestaurantList
    {
        public int RestaurantId { get; set; }
        public SelectList RestList { get; set; }

        public List<Restaurant> Restaurants { get; set; } = new List<Restaurant>();


    }
}
