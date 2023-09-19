using Group_BeanBooking.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace Group_BeanBooking.Data
{
    public class SeedData
    {
        #region fields
        protected readonly ApplicationDbContext _context;
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly RoleManager<IdentityRole> _rolesManager;

        #endregion

        public SeedData(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> rolesManager)
        {
            _context = context;
            _userManager = userManager;
            _rolesManager = rolesManager;
        }

        public async Task SeedDataMain()
        {
            
            await SeedRestaurant();
            await SeedSittingTypes();
            await SeedSittings();
            await SeedRestaurantArea();
            await SeedRolesAsync();
            await SeedUsers();
            await AddUserToRoles();

            //await SeedUsersinRoles();
            await SeedReservationStatuses();
            await SeedReservationsOrigin();

        }

        public async Task SeedReservationsOrigin()
        {
            List<ResevationOrigin> origins = new()
            {
                new ResevationOrigin { Name = "Online"},
                new ResevationOrigin { Name = "Phone"},
                new ResevationOrigin { Name = "Email"},
                new ResevationOrigin { Name = "Person"},  
                
            };

            foreach (var item in origins)
            {
                
                if (await _context.ResevationOrigins.FirstOrDefaultAsync(r => r.Name == item.Name) == null)
                {
                    await _context.ResevationOrigins.AddAsync(item);
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task SeedRolesAsync()
        {
            List<string> roles = new()
            {
                "Administrator", "Staff", "Customer", "Supplier"
            };

            foreach (var role in roles)
            {
                if ( await _rolesManager.FindByNameAsync(role) == null)
                {
                    await _rolesManager.CreateAsync(new IdentityRole { Name = role });
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task SeedUsers()
        {
            List<ApplicationUser> list = new()
            {
                new ApplicationUser {FirstName = "Admin" , LastName = "Admin" , PhoneNumber="0212345678", Email ="admin@beancafe.com" , PasswordHash = "Admin123." },
                new ApplicationUser {FirstName = "Staff" , LastName = "Staff" , PhoneNumber="0212345679", Email ="staff@beancafe.com" , PasswordHash = "Staff123."},
                new ApplicationUser {FirstName = "Customer" , LastName = "Customer" , PhoneNumber="0212345680", Email ="customer@beancafe.com" , PasswordHash = "Customer123." },
                new ApplicationUser {FirstName = "Supplier" , LastName = "Supplier" , PhoneNumber="0212345681", Email ="supplier@beancafe.com" , PasswordHash = "Supplier123."},

            };

            foreach (var item in list)
            {
                if (await _userManager.FindByEmailAsync(item.Email) == null)
                {
                    item.UserName = item.Email;
                    item.EmailConfirmed = true;
                    await _userManager.CreateAsync(item, item.PasswordHash);
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task AddUserToRoles()
        {
            foreach (var user in _userManager.Users.ToList())
            {
                if (user.Email == "admin@beancafe.com")
                {
                    await _userManager.AddToRoleAsync(user, "Administrator");
                }
                else if (user.Email == "staff@beancafe.com")
                {
                    await _userManager.AddToRoleAsync(user, "Staff");
                }
                else if (user.Email == "customer@beancafe.com")
                {
                    await _userManager.AddToRoleAsync(user, "Customer");
                }
                else if (user.Email == "supplier@beancafe.com")
                {
                    await _userManager.AddToRoleAsync(user, "Supplier");
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task SeedReservationStatuses()
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
                if (await _context.ReservationStatuses.FirstOrDefaultAsync(r=>r.Name == item.Name) == null )
                {
                    await _context.ReservationStatuses.AddAsync(item);
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task SeedRestaurant()
        {
            List<Restaurant> list = new()
            {
                new Restaurant { Name = "Opera Bar", Phone = "02 5020 5560" },
                new Restaurant { Name = "Grill'd", Phone = "02 3025 6210"}

            };

            foreach(var restaurant in list)
            {
                
                if(await _context.Restaurants.FirstOrDefaultAsync(r=> r.Name == restaurant.Name) == null)
                {
                    await _context.Restaurants.AddAsync(restaurant);
                } 
            }
            await _context.SaveChangesAsync();
        }

        public async Task SeedRestaurantArea()
        {
            List<RestaurantArea> list = new()
            {
                new RestaurantArea { Name = "Main"},
                new RestaurantArea { Name = "Outside"},
                new RestaurantArea { Name = "Balcony"}
            };

            var restaurants = await _context.Restaurants.ToListAsync();
            var restaAreas = await _context.ResturantAreas.ToListAsync();

            foreach(var item in list)
            {
                foreach (var restaurant in restaurants)
                {
                    if(restaAreas.FirstOrDefault(r=> r.RestaurantId == restaurant.Id && r.Name == item.Name) == null)
                    {
                        item.RestaurantId = restaurant.Id;
                        await _context.ResturantAreas.AddAsync(item);
                    }
                    
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task SeedSittingTypes()
        {
            List<SittingType> list = new()
            {
                new SittingType { Name = "Breakfast"},
                new SittingType { Name = "Lunch"},
                new SittingType { Name = "Dinner"}
            };

            foreach (var type in list)
            {
                if (await _context.SittingTypes.FirstOrDefaultAsync(st => st.Name == type.Name) == null)
                {
                    await _context.SittingTypes.AddAsync(type);
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task SeedSittings()
        {
            var start = "19/09/2023 7:00:00 AM";
            List<Sitting> list = new()
            {
                new Sitting { Name = "Continental Breakfast" , Closed = false , Start = DateTime.Now , End = DateTime.Now.AddHours(4) , Capacity= 40, TypeId = 1},
                new Sitting { Name = "Continental Lunch" , Closed = false , Start = DateTime.Now.AddHours(6) , End = DateTime.Now.AddHours(4) , Capacity= 50, TypeId = 1},
                new Sitting { Name = "Continental Dinner" , Closed = false , Start = DateTime.Now.AddHours(12) , End = DateTime.Now.AddHours(4) , Capacity= 50, TypeId = 1},
                new Sitting { Name = "Continental Breakfast" , Closed = false , Start = DateTime.Parse(start),Capacity= 40, TypeId = 1},
                new Sitting { Name = "Continental Lunch" , Closed = false , Start = DateTime.Parse(start) , Capacity= 50, TypeId = 2},
                new Sitting { Name = "Continental Dinner" , Closed = false , Start = DateTime.Parse(start) , Capacity= 50, TypeId = 3},
                
            };
            var restaurants = await _context.Restaurants.ToListAsync();
            var sittings = await _context.Sittings.ToListAsync();
            
            var listrestaurants = await _context.Restaurants
                .Include(r => r.Sittings).ToListAsync();

            
            
            //review each restaurant sitting areas
            foreach(var restaurant in listrestaurants)
            {
                //review if these sittings in the list exist in the restaurant
                foreach (var item in list)
                {
                    //repeat 90 times and create if they do not exist. 
                    for (int i = 0; i < 90; i++)
                    {
                foreach (var r in restaurants)
                        if (restaurant.Sittings.SingleOrDefault(r => r.Name == item.Name && r.Start == DateTime.Parse(start).AddDays(i) && r.RestaurantId == restaurant.Id) == null)
                        {
                            await _context.Sittings.AddAsync(new Sitting
                            {
                                Name = item.Name,
                                Closed = false,
                                Start = DateTime.Parse(start).AddDays(i),
                                End = DateTime.Parse(start).AddDays(90 - i).AddHours(4),
                                Capacity = item.Capacity,
                                TypeId = item.TypeId,
                                RestaurantId = restaurant.Id
                            }) ;
                            await _context.SaveChangesAsync();

                        }
                    }
                }
            }
            await _context.SaveChangesAsync();
            
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
