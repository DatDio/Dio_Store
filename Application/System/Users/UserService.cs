using Data.EF;
using Data.EF.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Security.Claims;
using ViewModel.Common;
using ViewModel.System;
using ViewModel.Users;

namespace Application.System.Users
{
	public class UserService : IUserService
	{
		private readonly Dio_StoreContext _context;
		private readonly SignInManager<Account> _signInManager;
		private readonly UserManager<Account> _userManager;
		private readonly IHttpContextAccessor _httpContextAccessor;
		public UserService(SignInManager<Account> signInManager,
			UserManager<Account> userManager,
			Dio_StoreContext context,
			  IHttpContextAccessor httpContextAccessor)
		{
			_signInManager = signInManager;
			_userManager = userManager;
			_context = context;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<ResultModel<string>> Authencate(LoginVM rq)
		{
			var result = await _signInManager.PasswordSignInAsync(rq.Email,
									   rq.Password, rq.RememberMe, lockoutOnFailure: false);
			var user = await _userManager.FindByEmailAsync(rq.Email);
			if (user == null)
			{
				//return "Email không đúng";
				return new ResultModel<string>()
				{
					IsSuccessed = false,
					Message = "Email không đúng hoặc chưa đăng kí",
				};
			}
			if (result.Succeeded)
			{
				var roles = await _userManager.GetRolesAsync(user);
				if (roles.Contains(SystemContants.RoleAdmin))
				{
					//return "Admin";
					return new ResultModel<string>()
					{
						IsSuccessed = true,
						Message = SystemContants.RoleAdmin,
					};
				}
				else
				{
					//return "User";
					return new ResultModel<string>()
					{
						IsSuccessed = true,
						Message = SystemContants.RoleUser,
					};
				}
			}
			else
			{
				//return "Mật khẩu không đúng";
				return new ResultModel<string>()
				{
					IsSuccessed = false,
					Message = "Mật khẩu không đúng",
				};
			}
		}

		public async Task<bool> Delete(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			if (user != null)
			{
				var result = await _userManager.DeleteAsync(user);
				if (result.Succeeded)
				{
					return true;
				}

			}
			return false;
		}

		public async Task<UserVM> GetById(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			if (user == null)
			{
				return null;
			}
			var roles = await _userManager.GetRolesAsync(user);
			var userVM = new UserVM()
			{
				Email = user.Email,
				PhoneNumber = user.PhoneNumber,
				FirstName = user.UserName,
				//Dob = user.Dob,
				Id = user.Id,
				//LastName = user.LastName,
				//UserName = user.UserName,
				Roles = roles
			};
			return userVM;
		}
		public async Task<ResultModel<bool>> Register(RegisterVM request)
		{
			var user = await _userManager.FindByNameAsync(request.Email);
			if (user != null)
			{
				return new ResultModel<bool>()
				{
					IsSuccessed = false,
					Message = "Tài khoản đã tồn tại"
				};
			}
			if (await _userManager.FindByEmailAsync(request.Email) != null)
			{
				return new ResultModel<bool>()
				{
					IsSuccessed = false,
					Message = "Email đã tồn tại"
				}; ;
			}

			user = new Account()
			{
				//AccountBirthday = request.Dob,
				Email = request.Email,
				//FirstName = request.FirstName,
				//LastName = request.LastName,
				UserName = request.UserName,
				//PhoneNumber = request.PhoneNumber
			};
			var result = await _userManager.CreateAsync(user, request.Password);
			if (result.Succeeded)
			{
				await _userManager.AddToRoleAsync(user, SystemContants.RoleUser);
				// Đăng ký thành công, đăng nhập và chuyển hướng đến trang mong muốn
				await _signInManager.SignInAsync(user, isPersistent: true);
				return new ResultModel<bool>()
				{
					IsSuccessed = true
				};
			}
			return new ResultModel<bool>()
			{
				IsSuccessed = false,
				Message = "Đăng kí không thành công, vui lòng thử lại sau"
			};
		}
		public Task<PagedResult<UserVM>> GetUsersPaging(GetUserPagingRequest request)
		{
			throw new NotImplementedException();
		}
		public async Task<ResultModel<bool>> Update(string id, UpdateUserVM request)
		{
			if (await _userManager.Users.AnyAsync(x => x.Email == request.Email && x.Id != id))
			{
				return new ResultModel<bool>()
				{
					IsSuccessed = false,
					Message = "Email đã tồn tại!"
				};
			}
			var user = await _userManager.FindByIdAsync(id.ToString());
			//user.Dob = request.Dob;
			user.Email = request.Email;
			//user.FirstName = request.FirstName;
			//user.LastName = request.LastName;

			user.PhoneNumber = request.PhoneNumber;

			var result = await _userManager.UpdateAsync(user);
			if (result.Succeeded)
			{
				return new ResultModel<bool>()
				{
					IsSuccessed = true,
				};
			}
			return new ResultModel<bool>()
			{
				IsSuccessed = false,
				Message = "Cập nhật không thành công!"
			};
		}

		public async Task<string> GetUserID()
		{
			var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
			return userId;
		}
	}
}
