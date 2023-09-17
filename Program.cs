using Group_BeanBooking.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Group_BeanBooking.Areas.Identity.Data;


var builder = WebApplication.CreateBuilder(args);
var MyAlloSpecificOrigins = "_myAlloSpecificOrigins";

// Add services to the container.

builder.Services.AddRazorPages();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(connectionString));

//var mySqlConnString = builder.Configuration.GetConnectionString("MySQLConnection");
//builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseMySQL(mySqlConnString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>() //https://learn.microsoft.com/en-us/aspnet/core/security/authorization/secure-data?view=aspnetcore-7.0
    .AddEntityFrameworkStores<ApplicationDbContext>();
 
builder.Services.AddControllersWithViews();

var app = builder.Build();

var scope = app.Services.CreateScope();

var services = scope.ServiceProvider;

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy(name: MyAlloSpecificOrigins,
//        policy =>
//        {
//            policy.WithOrigins("https://localhost:7113")
//            .AllowAnyHeader()
//            .AllowAnyMethod()
//            .AllowAnyOrigin();
//        });
//});



var handler = new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
    {
        // Allow any certificate, regardless of validity
        return true;
    }
};

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCors(MyAlloSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapRazorPages();

app.Run();
