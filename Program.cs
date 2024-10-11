using AptechVision.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
     .EnableSensitiveDataLogging()
           .EnableDetailedErrors());

// Configure Identity with roles
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
    options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Create Roles and a default admin user
await CreateRolesAndUsers(app);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.Use(async (context, next) =>
{
    Console.WriteLine($"Requested Path: {context.Request.Path}");
    await next();
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Define route configuration
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "orders",
    pattern: "Orders/{action=OrderList}/{id?}",
    defaults: new { controller = "Orders" });

app.MapRazorPages();

app.Run();

async Task CreateRolesAndUsers(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    string[] roleNames = { "Admin", "User" };

    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            // Create the roles and seed them to the database
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    // Create a super user who could maintain the web app
    var powerUser = new IdentityUser
    {
        UserName = "admin@example.com",
        Email = "admin@example.com",
    };

    string adminPassword = "Admin@123";

    if (await userManager.FindByEmailAsync("admin@example.com") == null)
    {
        var createPowerUser = await userManager.CreateAsync(powerUser, adminPassword);
        if (createPowerUser.Succeeded)
        {
            // Assign the new user the "Admin" role 
            await userManager.AddToRoleAsync(powerUser, "Admin");
        }
    }
}
