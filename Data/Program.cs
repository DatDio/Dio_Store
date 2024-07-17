using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Data.EF;
using Data.EF.Entities;
using Data.SeedingData;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<Dio_StoreContext>(option => 
option.UseSqlServer(builder.Configuration.GetConnectionString("Dio_Store")
, b => b.MigrationsAssembly("Data")));

builder.Services.AddIdentity<Account, Role>(options =>
{
	// Cấu hình các tùy chọn Identity tại đây nếu cần

})
	   .AddEntityFrameworkStores<Dio_StoreContext>()
	   .AddDefaultTokenProviders();
using (var scope = builder.Services.BuildServiceProvider().CreateScope())
{
	var services = scope.ServiceProvider;
	try
	{
		var userManager = services.GetRequiredService<UserManager<Account>>();
		await SeedData.Initialize(services, userManager);
	}
	catch (Exception ex)
	{
		var logger = services.GetRequiredService<ILogger<Program>>();
		logger.LogError(ex, "An error occurred seeding the DB.");
	}
}
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
