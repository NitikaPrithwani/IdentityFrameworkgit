using IdentityFramework.Areas.Identity.Data;
using IdentityFramework.Models;
using IdentityFramework.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IEmailSender, EmailSender>();


// Connection string for SQL Server
var connectionString = builder.Configuration.GetConnectionString("SQLServerIdentityConnection")
    ?? throw new InvalidOperationException("Connection string 'SQLServerIdentityConnection' not found.");

// Add DbContext to the services container
builder.Services.AddDbContext<IdentityFrameworkDbContext>(options =>
    options.UseSqlServer(connectionString));

// Configure Identity services
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredUniqueChars = 4;
})
.AddEntityFrameworkStores<IdentityFrameworkDbContext>()
.AddDefaultTokenProviders();

// Get the configuration
var configuration = builder.Configuration;

// Add Google Authentication
builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = configuration["Authentication:Google:ClientId"];
        options.ClientSecret = configuration["Authentication:Google:ClientSecret"];
        options.CallbackPath = "/signin-google"; 
    });


// Authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CreateRolePolicy", policy =>
            policy.RequireAssertion(context =>
                context.User.IsInRole("Admin") || context.User.HasClaim(c => c.Type == "Create Role" && c.Value == "Create Role")
            ));
});


// Add Razor Pages support
builder.Services.AddRazorPages();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Enable authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Map Razor Pages
app.MapRazorPages();

// Map controller routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
