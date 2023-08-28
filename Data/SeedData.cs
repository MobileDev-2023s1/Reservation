using Group_BeanBooking.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using ReservationSystem.Data;

namespace Group_BeanBooking.Data
{
    public class SeedData
    {
        private readonly ApplicationDbContext _context;
        private readonly List<Restaurant> _restaurants;
        private readonly List<RestaurantArea> _restaurantArea;
        private readonly List<SittingType> _sittingTypes;
        private readonly List<Sitting> _sittings;
        private readonly List<RestaurantTable> _tables;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _rolesManager;
        

        public SeedData(ApplicationDbContext context , UserManager<ApplicationUser> userManager,
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
        }


        public void SeedDataMain()
        {
            SeedRestaurant();
            SeedSittingTypes();
            SeedSittings();
            SeedRestaurantArea();
            SeedRoles();
            SeedUsers();
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

        public void SeedRoles()
        {
            List<string> roles = new()
            {
                "Administrator", "Staff", "Customer", "Supplier", "Owner"
            };

            foreach (var role in roles)
            {
                var result = _rolesManager.FindByNameAsync(role).Result;

                if (_rolesManager.FindByNameAsync(role).Result == null)
                {
                    _rolesManager.CreateAsync(new IdentityRole { Name = role });
                }
            }
            _context.SaveChanges();
            
        }
        public void SeedUsers()
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
                   _userManager.CreateAsync(item, item.PasswordHash);
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
