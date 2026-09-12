using Access_Authority.Data;
using Access_Authority.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("constr")));

// Identity
builder.Services.AddIdentity<Users, IdentityRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;

    options.User.RequireUniqueEmail = true;

    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Home/LogIn";
    options.AccessDeniedPath = "/Home/LogIn";
});
// Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

var app = builder.Build();


// ==========================================
// CREATE / RESET DEFAULT ADMIN PASSWORD
// ==========================================
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider
        .GetRequiredService<UserManager<Users>>();

    var user = await userManager.FindByNameAsync("admin");

    if (user == null)
    {
        user = new Users
        {
            UserName = "admin",
            Email = "admin@example.com",
            EmailConfirmed = true
        };

        var createResult = await userManager.CreateAsync(
            user,
            "Admin@321"
        );

        if (!createResult.Succeeded)
        {
            foreach (var error in createResult.Errors)
            {
                Console.WriteLine(
                    $"{error.Code}: {error.Description}");
            }
        }
    }
    else
    {
        // Existing admin ka password reset
        var token = await userManager.GeneratePasswordResetTokenAsync(user);

        var resetResult = await userManager.ResetPasswordAsync(
            user,
            token,
            "Admin@321"
        );

        if (!resetResult.Succeeded)
        {
            foreach (var error in resetResult.Errors)
            {
                Console.WriteLine(
                    $"{error.Code}: {error.Description}");
            }
        }
        else
        {
            Console.WriteLine("Admin password changed successfully.");
        }
    }
}

// ==========================================
// HTTP REQUEST PIPELINE
// ==========================================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

// Session
app.UseSession();

// Identity Authentication
app.UseAuthentication();

// Identity Authorization
app.UseAuthorization();


// ==========================================
// ROUTE
// ==========================================
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();