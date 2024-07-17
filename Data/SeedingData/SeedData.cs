using Microsoft.AspNetCore.Identity;
using Data.EF.Entities;

namespace Data.SeedingData
{
	public static class SeedData
	{
		public static async Task Initialize(IServiceProvider serviceProvider, UserManager<Account> userManager)
		{
			var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();

			string[] roleNames = { "Admin", "User" };
			IdentityResult roleResult;

			foreach (var roleName in roleNames)
			{
				var roleExist = await roleManager.RoleExistsAsync(roleName);
				if (!roleExist)
				{
					roleResult = await roleManager.CreateAsync(new Role(roleName));
				}
			}

			// Create a default Admin user
			var adminUser = new Account
			{
				UserName = "admin123@gmail.com",
				Email = "admin123@gmail.com",
				EmailConfirmed = true
			};

			string adminPassword = "Admin@123";
			var user = await userManager.FindByEmailAsync(adminUser.Email);

			if (user == null)
			{
				var createAdminUser = await userManager.CreateAsync(adminUser, adminPassword);
				if (createAdminUser.Succeeded)
				{

					await userManager.AddToRoleAsync(adminUser, "Admin");
				}
			}
		}
	}

}
