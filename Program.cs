using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MvcSaed.Data;
using MvcSaed.Models;
using MvcSaed.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("MvcSaedContext") ?? throw new InvalidOperationException("Connection string 'MvcSaedContext' not found.");
builder.Services.AddDbContext<MvcSaedContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 25))));

// Configurar Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Configurações de senha
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Configurações de lockout
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // Configurações de usuário
    options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;

    // Configurações de sign-in
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
})
.AddEntityFrameworkStores<MvcSaedContext>()
.AddDefaultTokenProviders();

// Configurações de cookies
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.LogoutPath = "/Account/Logout";
    options.SlidingExpiration = true;
});

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Apply pending migrations and seed data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MvcSaedContext>();
    db.Database.Migrate();
    
    // Initialize seed data
    await SeedDataService.Initialize(app.Services);
}

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
    name: "default",
    pattern: "{controller=Pessoas}/{action=Index}/{id?}");

app.Run();
