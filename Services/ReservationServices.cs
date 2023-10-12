﻿using Group_BeanBooking.Areas.Customers.Models.Bookings;
using Group_BeanBooking.Areas.Identity.Data;
using Group_BeanBooking.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Group_BeanBooking.Areas.Customers.Models.Bookings;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Humanizer;

namespace Group_BeanBooking.Services
{
    public class ReservationServices : ServicesArea
    {
        public ReservationServices(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> rolesManager) : base(context, userManager, rolesManager)
        {
            
        }

        #region DB queries for reservations
       public async Task<ResevationOrigin> GetResevationOrigins(string reservationOriginName)
        {
            return await _context.ResevationOrigins.FirstOrDefaultAsync(r => r.Name == reservationOriginName);
        }

        public async Task<List<Reservation>> GetAllReservations()
        {
            var reservations = await _context.Reservations
                .Include(r => r.Person) //eager loading
                .Include(r => r.Sitting) //keyless entities mapping them to the result set of store procedure
                    .ThenInclude(s => s.Restaurant)
                .Include(r => r.ResevationOrigin)
                .Include(r => r.ReservationStatus)
                .OrderBy(r => r.Start)
                .ToListAsync();

            return reservations;
        }



        /// <summary>
        /// Gets all the reservation that are not cancelled and are 
        /// </summary>
        /// <returns></returns>
        public async Task<List<Reservation>> GetActiveReservationsByMonth(DateTime start, DateTime end)
        {
            var reservations = await _context.Reservations
                .Include(r => r.Person) //eager loading
                .Include(a => a.RestaurantArea)
                /*.Include(r => r.ReservationStatus)*/ //185 read without - 437 with this
                .Where(r=>r.ReservationStatusID != 3 && r.ReservationStatusID != 5)
                .Where(r => r.Start >= start && r.Start <= end)
                .OrderBy(r => r.Start)
                .ToListAsync();

            return reservations;

        }
               
        //https://learn.microsoft.com/en-us/ef/core/querying/related-data/eager
        public async Task<List<Reservation>> GetReservationsByPersonId(int? personId)
        {
            var reservations = await _context.Reservations
                .Include(r => r.Person) //eager loading
                .Include(r => r.Sitting) //keyless entities mapping them to the result set of store procedure
                    .ThenInclude(s => s.Restaurant)
                .Include(r => r.ResevationOrigin)
                .Include(r => r.ReservationStatus)
                .Where(r => r.PersonId == personId && r.Start >= DateTime.Now 
                        && r.ReservationStatusID != 3 && r.ReservationStatusID != 5)
                .OrderBy(r => r.Start)
                .ToListAsync();

            return reservations;
        }

        public async Task<Reservation> GetReservationsByReservationId(int reservationId)
        {
            var reservations = await _context.Reservations
                .Include(r => r.Person) //eager loading
                .Include(r => r.Sitting) //keyless entities mapping them to the result set of store procedure
                    .ThenInclude(s => s.Restaurant)
                    .ThenInclude(ra => ra.RestaurantAreas)
                .Include(r => r.ResevationOrigin)
                .Include(r => r.ReservationStatus)
                .SingleAsync(p => p.Id == reservationId);

            return reservations;
        }

        #endregion

        public async Task<Reservation> CreateReservation(Person person, Create c)
        {
            var reservation = new Reservation
            {
                Start = c.Starttime,
                Duration = c.Duration,
                PersonId = person.Id,
                SittingID = c.SittingId,
                ReservationStatusID = 1,
                ResevationOrigin = await GetResevationOrigins("online"),
                Guests = c.Guests,
                Comments = c.Comments,
                RestaurantAreaId = c.RestaurantAreaId
            };

            await _context.Reservations.AddAsync(reservation);
            await _context.SaveChangesAsync();

            return reservation;

        }

        public async Task EditReservation(Edit c)
        {
            var currentRes = await GetReservationsByReservationId(c.ReservationId);
           
            await _context.Reservations
                    .Where(b => b.Id == c.ReservationId)
                    .ExecuteUpdateAsync(b => b
                        .SetProperty(b => b.Start, c.Starttime)
                        .SetProperty(b => b.Duration, c.Duration)
                        .SetProperty(b => b.Guests, c.Guests)
                        .SetProperty(b => b.SittingID, c.SittingId)
                        .SetProperty(b => b.PersonId, currentRes.PersonId)
                        .SetProperty(b => b.RestaurantAreaId, c.RestaurantAreaId)
                        .SetProperty(b => b.Comments, c.Comments)                        
                        );
                       
        }

        public async Task DeleteReservation(int id)
        {
            await _context.Reservations
                .Where(r => r.Id == id)
                .ExecuteUpdateAsync(r => r
                    .SetProperty(r => r.ReservationStatusID, 3)
                );
        }

    }
}
