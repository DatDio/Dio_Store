using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Data.EF.Entities;
using Data.EF;
using Application.Product;
using Application.Files;
using Application.Categories;
using Application.System.Users;
using Application.Cart;
using Application.Order;
using Application.Payments.VNPay;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<Dio_StoreContext>(option =>
option.UseSqlServer(builder.Configuration.GetConnectionString("Dio_Store")
, b => b.MigrationsAssembly("Data")));
// Add services to the container.

builder.Services.AddControllersWithViews();

builder.Services.AddIdentity<Account, Role>(options =>
{
	// Cấu hình các tùy chọn Identity tại đây nếu cần
	options.Password.RequireDigit = false;
	options.Password.RequiredLength = 6;
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequireUppercase = false;
	options.Password.RequireLowercase = false;
	options.SignIn.RequireConfirmedEmail = false;
	options.Lockout.AllowedForNewUsers = false;
})
	   .AddEntityFrameworkStores<Dio_StoreContext>()
	   .AddDefaultTokenProviders();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IVNPayService, VNPayService>();

builder.Services.AddScoped<IStorageService, FileStorageService>();
builder.Services.AddScoped<IProductServicecs, ProductService>();
builder.Services.AddScoped<ICatetgoryServicecs, CategoryService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();

//Cấu hình login by google,fb

builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
	googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
	googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
});


builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(30);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});
builder.Services.ConfigureApplicationCookie(options =>
{
	options.Cookie.HttpOnly = true;
	options.ExpireTimeSpan = TimeSpan.FromDays(30); // Thời gian hết hạn của cookie
	options.LoginPath = "/Account/Login";
	options.AccessDeniedPath = "/Account/AccessDenied";
	options.SlidingExpiration = true;
});
// Ensure the database is created and seed roles/users


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
app.UseSession();
app.UseCookiePolicy();
app.UseAuthentication(); 
app.UseAuthorization();


// Cấu hình routing cho dự án Admin

app.MapControllerRoute(
	name: "areas",
	pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
