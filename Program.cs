using Microsoft.EntityFrameworkCore;
using Aspnet_Project.Models;
using Microsoft.AspNetCore.Identity;
using Aspnet_Projects.Services;

var builder = WebApplication.CreateBuilder(args);

//Add sessions
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add services to the container.
builder.Services.AddControllersWithViews();

//Add MYSql
var connectionString = builder.Configuration.GetConnectionString("Default") ?? throw new InvalidOperationException("Connection string not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseMySQL(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddTransient<DbInitializer>();

builder.Services.AddAuthentication().AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
});

builder.Services.AddScoped<CartService>();

// Add so controllers can read config
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

var app = builder.Build();

// Enable Sessions on our requests


app.UseSession();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "Brief",
    pattern: "Brief",
    defaults: new { controller = "Home", action = "Brief" });

app.MapControllerRoute(
    name: "Products",
    pattern: "Products",
    defaults: new { controller = "Products", action = "Products" });



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using var scope = scopeFactory.CreateScope();
var initializer = scope.ServiceProvider.GetRequiredService<DbInitializer>();
await DbInitializer.Initialize(
    scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>(),
    scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>()
);
app.Run();
