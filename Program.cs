using Microsoft.EntityFrameworkCore;
using Bookstore.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddMemoryCache();
builder.Services.AddSession();

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BookstoreContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("BookstoreContext")));

/*
Configure Identity service to web host, and reset the password policy.
Identity info are saved in stores, and in this case, the stores are located in BookstoreContext (database).
Dotnet Identity service is a token-based security protocol, means an identity is represented by a token.
*/
builder.Services.AddIdentity<User, IdentityRole>(options => {
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
}).AddEntityFrameworkStores<BookstoreContext>() 
  .AddDefaultTokenProviders();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseRouting();

/*
configure the middleware for authentication and authorization. 
Note they must be after the starting point where routing starts.
*/
app.UseAuthentication();   
app.UseAuthorization();

/*
If the admin as a user is not existing, then create one and store in Identity stores. 
Creating the admin user is a one-time operation that is executed outside of HTTP request. 
Such service, like DbContext or Identity managers, are scoped services, which means 
they are executed once at applications startup and are disposed at application shutdown.
The other lifetime options, in addition to scoped, are Singleton(one instance in one app) and Transient (one instance created at each request). 
*/
var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using (var scope = scopeFactory.CreateScope())
{
    await ConfigureIdentity.CreateAdminUserAsync(scope.ServiceProvider); //Admin user is NOT executed along with other seeding data
}

app.UseSession();

app.MapAreaControllerRoute(
    name: "admin",
    areaName: "Admin",
    pattern: "Admin/{controller=Book}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "page_sort",
    pattern: "{controller}/{action}/page/{pagenumber}/size/{pagesize}/sort/{sortfield}/{sortdirection}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}/{slug?}");

app.Run();
