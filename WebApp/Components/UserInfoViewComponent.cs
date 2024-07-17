using Data.EF.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ViewModel.Users;

namespace WebApp.Components
{
	public class UserInfoViewComponent : ViewComponent
	{
		private readonly UserManager<Account> _userManager;

		public UserInfoViewComponent(UserManager<Account> userManager)
		{
			_userManager = userManager;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			if (User.Identity.IsAuthenticated)
			{
				var user = await _userManager.GetUserAsync(UserClaimsPrincipal);
				if (user != null)
				{
					var roles = await _userManager.GetRolesAsync(user);
					var model = new UserVM
					{
						UserName = user.UserName,
						Email = user.Email,
						Roles = roles
					};
					return View(model);
				}
			}
			return View((UserVM)null);
		}
	}
}
