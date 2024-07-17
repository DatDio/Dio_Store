using Application.Categories;
using Application.Product;
using Data.EF.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ViewModel.Products;
using ViewModel.Users;

namespace WebApp.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class HomeController : Controller
	{
		private readonly IProductServicecs _productService;
		private readonly ICatetgoryServicecs _categoryService;
		private readonly SignInManager<Account> _signInManager;
		private readonly UserManager<Account> _userManager;
		public HomeController(IProductServicecs productService,
			ICatetgoryServicecs catetgoryServicecs,
			SignInManager<Account> signInManager,
			UserManager<Account> userManager)
		{
			_productService = productService;
			_categoryService = catetgoryServicecs;
			_signInManager = signInManager;
			_userManager = userManager;
		}
		// GET: HomeController
		public async Task<ActionResult> Index()
		{
			if (User.Identity.IsAuthenticated)
			{
				// Lấy thông tin người dùng hiện tại
				var user = await _userManager.GetUserAsync(User);
				if (user != null)
				{
					// Lấy danh sách các role của người dùng
					var roles = await _userManager.GetRolesAsync(user);

					var model = new UserVM
					{
						UserName = user.UserName,
						Email = user.Email,
						Roles = roles
					};

					// Truyền model vào view
					return View(model);
				}
			}
			return RedirectToAction("Login", "Account");
		}

		// GET: HomeController/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		// GET: HomeController/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: HomeController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(IFormCollection collection, ProductCreateVM rq)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: HomeController/Edit/5
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: HomeController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: HomeController/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: HomeController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}
	}
}
