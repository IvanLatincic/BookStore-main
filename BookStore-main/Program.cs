using BookStore.Data;
using BookStore.Data.Cart;
using BookStore.Data.Repositories;
using BookStore.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;


var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);
    // NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
    builder.Host.UseNLog();

    // Add services to the container.
    builder.Services.AddControllersWithViews();
    builder.Services.AddRazorPages();
    builder.Services.AddDbContext<BookStoreDbContext>(options => { options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); });
    builder.Services.AddIdentity<ApplicationUser, IdentityRole>(/*options => { options.SignIn.RequireConfirmedEmail = true; }*/).AddEntityFrameworkStores<BookStoreDbContext>().AddDefaultTokenProviders();
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    builder.Services.AddScoped(x => ShoppingCart.GetCart(x));
    builder.Services.AddSession();
    builder.Services.AddMemoryCache();
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    }).AddGoogle(options =>
    {
        options.ClientId = "535563586038-b6p7t8fpi2e09tuhe6mku2vo6hqr8873.apps.googleusercontent.com";
        options.ClientSecret = "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";
    }).AddFacebook(options =>
    {
        options.AppId = "1664613534040654";
        options.AppSecret = "254398e7ab13ff4d3989ebae9ec83803";
    });

    builder.Services.AddAuthorization();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
        app.UseStatusCodePagesWithReExecute("/Error/{0}");
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();
    app.UseSession();
    app.UseAuthorization();
    app.UseAuthentication();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Product}/{action=Index}/{id?}");

    AppDbInitializier.SeedUsersAndRolesAsync(app).Wait();

    app.Run();
}

catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}

finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}