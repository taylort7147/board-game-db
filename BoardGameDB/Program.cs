using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Identity;
using BoardGameDB.Areas.Identity.Data;
using BoardGameDB.Areas.Identity.Authorization;
using Microsoft.AspNetCore.Authorization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var connectionString = Environment.GetEnvironmentVariable("BGDB_CONNECTION_STRING")
    ?? builder.Configuration.GetConnectionString("BoardGameDBContextConnection");
var identityConnectionString = Environment.GetEnvironmentVariable("BGDB_IDENTITY_CONNECTION_STRING")
    ?? builder.Configuration.GetConnectionString("BoardGameDBIdentityDbContextConnection");

builder.Services.AddDbContext<BoardGameDB.Data.BoardGameDBContext>(options => options.UseSqlite(connectionString));
builder.Services.AddDbContext<BoardGameDBIdentityDbContext>(options => options.UseSqlite(identityConnectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<BoardGameDBIdentityDbContext>();

builder.Services.AddMvc();
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(
        Policy.ReadWrite, 
        policy => policy.RequireRole(
            Role.Administrator,
            Role.Editor));
});

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);

    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BoardGameDB.Data.BoardGameDBContext>();
    db.Database.Migrate();
    db.Database.EnsureCreated();
}

static async Task CreateRoleIfMissingAsync(RoleManager<IdentityRole> roleManager, string role)
{
    if (!await roleManager.RoleExistsAsync(role))
    {
        await roleManager.CreateAsync(new IdentityRole(role));
    }
}


// Set up identity DB
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BoardGameDB.Areas.Identity.Data.BoardGameDBIdentityDbContext>();
    db.Database.Migrate();
    db.Database.EnsureCreated();

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await CreateRoleIfMissingAsync(roleManager, Role.Editor);
    await CreateRoleIfMissingAsync(roleManager, Role.Administrator);
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Games}/{action=Index}/{id?}");
    endpoints.MapControllerRoute(
        name: "api",
        pattern: "api/{controller=Games}/{action=Index}/{id?}");
});

app.Run();
