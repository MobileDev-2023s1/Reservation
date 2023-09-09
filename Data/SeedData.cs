using Group_BeanBooking.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Group_BeanBooking.Data;
using System.Collections.Generic;

namespace Group_BeanBooking.Data
{
    public class SeedData
    {
        #region fields
        protected readonly ApplicationDbContext _context;
        protected readonly List<Restaurant> _restaurants;
        protected readonly List<RestaurantArea> _restaurantArea;
        protected readonly List<SittingType> _sittingTypes;
        protected readonly List<Sitting> _sittings;
        protected readonly List<RestaurantTable> _tables;
        protected readonly List<ReservationStatus> _reservationStatus;
        protected readonly List<ResevationOrigin> _reservationOrigin;

        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly RoleManager<IdentityRole> _rolesManager;

        #endregion

        public SeedData(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> rolesManager)
        {
            _context = context;
            _userManager = userManager;
            _rolesManager = rolesManager;
            _restaurants = _context.Restaurants.ToList();
            _restaurantArea = _context.ResturantAreas.ToList();
            _sittingTypes = _context.SittingTypes.ToList();
            _sittings = _context.Sittings.ToList();
            _tables = _context.RestaurantTables.ToList();
            _reservationStatus = _context.ReservationStatuses.ToList();
            _reservationOrigin = _context.ResevationOrigins.ToList();
        }

        public async Task SeedDataMain()
        {
            var list = _userManager.Users.ToList();
            var usersInRole = _context.UserRoles.ToList();

            SeedRestaurant();
            SeedSittingTypes();
            SeedSittings();
            SeedRestaurantArea();
            await SeedRolesAsync();
            await SeedUsers();
            SeedSupplierinRoles(list[2], usersInRole);
            SeedStaffinRoles(list[3], usersInRole);
            SeedAdministratorinRoles(list[1], usersInRole);
            SeedCustomerinRoles(list[0], usersInRole);

            //await SeedUsersinRoles();
            SeedReservationStatuses();
            SeedReservationsOrigin();

        }

        public void SeedReservationsOrigin()
        {
            List<ResevationOrigin> origins = new()
            {
                new ResevationOrigin { Name = "Phone"},
                new ResevationOrigin { Name = "Email"},
                new ResevationOrigin { Name = "Person"},                
            };

            foreach (var item in origins)
            {
                if (_reservationOrigin.FirstOrDefault(r => r.Name == item.Name) == null)
                {
                    _context.ResevationOrigins.Add(item);
                }
            }
            _context.SaveChanges();
        }

        public void SeedSupplierinRoles(ApplicationUser user , List<IdentityUserRole<string>> roles)
        {
            if(roles.FirstOrDefault(u=> u.UserId ==  user.Id) == null)
            {
                 _userManager.AddToRoleAsync(user, "Supplier");
            }
        }

        public void SeedStaffinRoles(ApplicationUser user, List<IdentityUserRole<string>> roles)
        {
            if (roles.FirstOrDefault(u => u.UserId == user.Id) == null)
            {
                 _userManager.AddToRoleAsync(user, "Staff");
            }
        }

        public void SeedAdministratorinRoles(ApplicationUser user, List<IdentityUserRole<string>> roles)
        {
            if (roles.FirstOrDefault(u => u.UserId == user.Id) == null)
            {
                _userManager.AddToRoleAsync(user, "Administrator");
            }
        }

        public void SeedCustomerinRoles(ApplicationUser user, List<IdentityUserRole<string>> roles)
        {
            if (roles.FirstOrDefault(u => u.UserId == user.Id) == null)
            {
                _userManager.AddToRoleAsync(user, "Customer");
            }
        }

        public void SeedReservationStatuses()
        {
       
            List<ReservationStatus> list = new()
            {
                new ReservationStatus {Name = "Pending"},
                new ReservationStatus {Name = "Confirmed"},
                new ReservationStatus {Name = "Cancelled"},
                new ReservationStatus {Name = "Seated"},
                new ReservationStatus {Name = "Completed"}
            };

            foreach(var item in list) 
            {
                if (_reservationStatus.FirstOrDefault(r=>r.Name == item.Name) == null )
                {
                    _context.ReservationStatuses.Add(item);
                }
            }
            _context.SaveChanges();
        }

        public void SeedRestaurant()
        {
            List<Restaurant> list = new()
            {
                new Restaurant { Name = "Opera Bar", Phone = "02 5020 5560" },
                new Restaurant { Name = "Grill'd", Phone = "02 3025 6210"}

            };

            foreach(var restaurant in list)
            {
                if(_restaurants.FirstOrDefault(r=> r.Name == restaurant.Name) == null)
                {
                    _context.Add(restaurant);
                    _context.SaveChanges();
                } 
            }
        }

        public void SeedRestaurantArea()
        {
            List<RestaurantArea> list = new()
            {
                new RestaurantArea { Name = "Main"},
                new RestaurantArea { Name = "Outside"},
                new RestaurantArea { Name = "Balcony"}
            };

            foreach (var area in list)
            {
                foreach(var rest in _restaurants)
                {
                    if(_restaurantArea.Where(ra => ra.Name == area.Name && ra.RestaurantId == rest.Id).FirstOrDefault() == null)
                    {
                        rest.RestaurantAreas.Add(area);
                        _context.Add(area);
                    }

                }
                _context.SaveChanges();
            }
        }
                
        public void SeedSittingTypes()
        {
            List<SittingType> list = new()
            {
                new SittingType { Name = "Breakfast"},
                new SittingType { Name = "Lunch"},
                new SittingType { Name = "Dinner"}
            };

            foreach (var type in list)
            {
                if(_sittingTypes.FirstOrDefault(st => st.Name == type.Name) == null)
                {
                    _context.Add(type);
                }
                else
                {

                }
                _context.SaveChanges();
            }
        }

        public void SeedSittings()
        {
            
            List<Sitting> list = new()
            {
                new Sitting { Name = "Continental Breakfast" , Closed = false , Start = DateTime.Now , End = DateTime.Now.AddHours(4) , Capacity= 40, TypeId = 1}
            };

            foreach(var r  in _restaurants)
            {
                if(_sittings.FirstOrDefault(st => st.RestaurantId == r.Id) == null)
                {
                    r.Sittings.Add(list[0]);
                }
            }

            foreach (var type in list)
            {
                if (_sittings.FirstOrDefault(st => st.Name == type.Name) == null)
                {
                    _context.Add(type);
                }
                _context.SaveChanges();
            }


        }

        public async Task SeedRolesAsync()
        {
            List<string> roles = new()
            {
                "Administrator", "Staff", "Customer", "Supplier"
            };

            foreach (var role in roles)
            {
                if (_rolesManager.FindByNameAsync(role).Result == null)
                {
                    await _rolesManager.CreateAsync(new IdentityRole { Name = role });
                }
            }
            _context.SaveChanges();
            
        }
        public async Task SeedUsers()
        {
            List<ApplicationUser> list = new()
            {
                new ApplicationUser {FirstName = "Admin" , LastName = "Admin" , PhoneNumber="0212345678", Email ="admin@beancafe.com" , PasswordHash = "Admin123." },
                new ApplicationUser {FirstName = "Staff" , LastName = "Staff" , PhoneNumber="0212345679", Email ="staff@beancafe.com" , PasswordHash = "Staff123."},
                new ApplicationUser {FirstName = "Customer" , LastName = "Customer" , PhoneNumber="0212345680", Email ="Customer@beancafe.com" , PasswordHash = "Customer123." },
                new ApplicationUser {FirstName = "Supplier" , LastName = "Supplier" , PhoneNumber="0212345681", Email ="Supplier@beancafe.com" , PasswordHash = "Supplier123."},

            };

            foreach(var item in list)
            {
                if(_userManager.FindByEmailAsync(item.Email).Result == null)
                {
                   item.UserName = item.Email;
                   item.EmailConfirmed = true;
                   await _userManager.CreateAsync(item, item.PasswordHash);
                }
            }
        }

        public void SeedTables()
        {
            //see which areas are included in the restaurant
            var list = new List<RestaurantTable>()
            {
                new RestaurantTable {Name = "M1" , RestaurantAreaId = 1 },
                new RestaurantTable {Name = "M2" , RestaurantAreaId = 1 },
                new RestaurantTable {Name = "M3" , RestaurantAreaId = 1 },
                new RestaurantTable {Name = "M4" , RestaurantAreaId = 1 },
                new RestaurantTable {Name = "M5" , RestaurantAreaId = 1 },
                new RestaurantTable {Name = "O1" , RestaurantAreaId = 2 },
                new RestaurantTable {Name = "O2" , RestaurantAreaId = 2 },
                new RestaurantTable {Name = "O3" , RestaurantAreaId = 2 },
                new RestaurantTable {Name = "O4" , RestaurantAreaId = 2 },
                new RestaurantTable {Name = "O5" , RestaurantAreaId = 2 },

                
            };
            

        }
    }
}
