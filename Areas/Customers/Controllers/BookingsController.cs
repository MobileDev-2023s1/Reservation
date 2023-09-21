﻿using Group_BeanBooking.Areas.Identity.Data;
using Group_BeanBooking.Data;
using Group_BeanBooking.Areas.Customers.Models.Bookings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Group_BeanBooking.Services;
using Microsoft.EntityFrameworkCore;
using Humanizer;

namespace Group_BeanBooking.Areas.Customers.Controllers
{

    public class BookingsController : CustomersAreaController
    {
        
        private readonly RestaurantServices _restaurantServices;
        private readonly PersonServices _personServices;
        private readonly ReservationServices _reservationServices;

        public BookingsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> rolesManager) : base(context, userManager, rolesManager)
        {
            
            _restaurantServices = new RestaurantServices(context, userManager,rolesManager);
            _personServices = new PersonServices(context, userManager,rolesManager);
            _reservationServices = new ReservationServices(context, userManager,rolesManager);  
        }
        
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create(int id)
        {
           
            var restaurant = await _context.Restaurants
                .Include(s => s.Sittings)
                .Include(a => a.RestaurantAreas)
                .SingleOrDefaultAsync(r => r.Id == id);

            var c = new Create()
            {
                RestaurantId = id,
                RestaurantName = restaurant.Name,
                SittingAreaList = new SelectList(restaurant.RestaurantAreas, "Id", "Name"),
                SittingList = new SelectList(restaurant.Sittings, "Id", "Name")
            };
            
            return View(c);
        }


        [HttpGet]
        public async Task<List<Sitting>> GetRestuarantData(int Id, string Date)
        {
            var timeBooking = DateTime.Parse(Date);
            
            
            var Time = DateTime.Now;
            var end = timeBooking.AtMidnight().AddMicroseconds(1);

            var sitting = await _context.Sittings
                .Include(r => r.Restaurant)
                .Where(r => r.RestaurantId == Id)
                .Where(s => s.Start >= timeBooking && s.End < end).ToListAsync();

            return sitting;

            //var restaurant = await _context.Restaurants
            //    //.Include(s => s.Sittings)
            //    .Include(a => a.RestaurantAreas)
            //    .SingleOrDefaultAsync(restaurant => restaurant.Id == Id);

            //return restaurant;

        }



        [HttpPost]
        public async Task<IActionResult> Create(Group_BeanBooking.Areas.Customers.Models.Bookings.Create c)
        {
            var person = await _personServices.UserValidation(c, null);

            if(ModelState.IsValid)
            {
                await _reservationServices.CreateReservation(person, c);
                TempData["AlertMessage"] = "Booking created successfully";
            }

            return RedirectToAction("Details", "Bookings" , new  { id = person.Id , area="Customers"});
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            var model = new Group_BeanBooking.Areas.Customers.Models.Bookings.Details();
            if(id != null)
            {
                model.Reservations = await _reservationServices.GetReservationsByPersonId(id);
            } else if(User.Identity.IsAuthenticated)
            {
                var person = await _context.People.SingleOrDefaultAsync(u => u.Email == User.Identity.Name);
                model.Reservations = await _reservationServices.GetReservationsByPersonId(person.Id);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Details(string? email , int? id) 
        {
            var model = new Group_BeanBooking.Areas.Customers.Models.Bookings.Details();
            Person user = new();
            List<Reservation> bookings = new();           

            if (ModelState.IsValid)
            {
                //person has not logged in and then email is used for search
                if(id == null)
                {   var u = await _personServices.GetPersonByEmail(email);

                    if(u != null) {bookings = await _reservationServices.GetReservationsByPersonId(u.Id);}
                }
                else
                {
                    bookings = await _reservationServices.GetReservationsByPersonId(id);
                }
            }           

            model.Reservations = bookings;
            model.Person = user;
            model.Email = email;
            
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var reservation = await _reservationServices.GetReservationsByReservationId(id);
            var areas = await _restaurantServices.GetRestaurantAreaByRestaurantId(reservation.Sitting.RestaurantId);
            var sittings = await _restaurantServices.GetSittingsByRestaurantId(reservation.Sitting.RestaurantId);

            var model = new Edit
            {
                ReservationId = reservation.Id,
                RestaurantName = reservation.Sitting.Restaurant.Name,
                PersonId = reservation.Person.Id,
                FirstName = reservation.Person.FirtName,
                LastName = reservation.Person.LastName,
                PhoneNumber = reservation.Person.Phone,
                Email = reservation.Person.Email,
                Guests = reservation.Guests,
                Comments = reservation.Comments,
                Starttime = reservation.Start,
                Duration = reservation.Duration,
                SittingId = reservation.SittingID,
                RestaurantAreaId = reservation.RestaurantAreaId,
                SittingAreaName = areas.Single(a => a.Id == reservation.RestaurantAreaId).Name,
                SittingName = sittings.Single(a => a.Id == reservation.SittingID).Name,
                SittingAreaList = new SelectList(areas, "Id", "Name", new { CurrentId = reservation.RestaurantAreaId }),
                SittingList = new SelectList(sittings, "Id", "Name")
            };   

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Edit c)
        {
            if (ModelState.IsValid)
            {
                await _reservationServices.EditReservation(c);
                await _context.SaveChangesAsync();
                TempData["AlertMessage"] = "Booking modified successfully";
            }

            return RedirectToAction("Details", "Bookings", new {area= "Customers" });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var reservation = await _reservationServices.GetReservationsByReservationId(id);
            var areas = await _restaurantServices.GetRestaurantAreaByRestaurantId(reservation.Sitting.RestaurantId);
            var sittings = await _restaurantServices.GetSittingsByRestaurantId(reservation.Sitting.RestaurantId);

            var model = new Edit
            {
                ReservationId = reservation.Id,
                RestaurantName = reservation.Sitting.Restaurant.Name,
                PersonId = reservation.Person.Id,
                FirstName = reservation.Person.FirtName,
                LastName = reservation.Person.LastName,
                PhoneNumber = reservation.Person.Phone,
                Email = reservation.Person.Email,
                Guests = reservation.Guests,
                Comments = reservation.Comments,
                Starttime = reservation.Start,
                Duration = reservation.Duration,
                SittingId = reservation.SittingID,
                RestaurantAreaId = reservation.RestaurantAreaId,
                SittingAreaName = areas.Single(a => a.Id == reservation.RestaurantAreaId).Name,
                SittingName = sittings.Single(a => a.Id == reservation.SittingID).Name,
                SittingAreaList = new SelectList(areas, "Id", "Name", new { CurrentId = reservation.RestaurantAreaId }),
                SittingList = new SelectList(sittings, "Id", "Name")
            };

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            if (ModelState.IsValid)
            {
                await _reservationServices.DeleteReservation(id);
                TempData["AlertMessage"] = "Booking deleted successfully";
            }

            return RedirectToAction("Details", "Bookings", new { area = "Customers" });
        }

    }
}
