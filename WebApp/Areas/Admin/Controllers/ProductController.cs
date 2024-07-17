using Application.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Data.EF;
using ViewModel.Products;
using ViewModel.ProductImages;
using Data.EF.Entities;
using Application.Categories;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.AspNetCore.Mvc.ModelBinding;
//using Dio_Store.Application.Products;


namespace WebApp.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class ProductController : Controller
	{
		private readonly IProductServicecs _productService;
		private readonly ICatetgoryServicecs _categoryService;
		public ProductController(IProductServicecs productService, ICatetgoryServicecs catetgoryServicecs)
		{
			_productService = productService;
			_categoryService = catetgoryServicecs;

		}

		// GET: ProductControllerl
		public async Task<ActionResult> Index(int pageIndex = 1, int pageSize = 20)
		{
			try
			{
				var pagedResult = await _productService.GetPagingProducts(pageIndex, pageSize);
				return View(pagedResult);
			}
			catch
			{

			}
			return BadRequest();
		}

		// GET: ProductController/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		// GET: ProductController/Create
		public async Task<ActionResult> Create()
		{
			var categories = await _categoryService.GetAll();
			var model = new ProductCreateVM
			{
				Categories = categories
			};
			return View(model);
		}

		// POST: ProductController/Create
		[HttpPost]

		//[Consumes("multipart/form-data")]
		public async Task<ActionResult> Create(ProductCreateVM rq, List<IFormFile> images)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var productID = await _productService.Create(rq);
					if (productID == 0)
					{
						ModelState.AddModelError(string.Empty, "Có lỗi xảy ra");
						rq.Categories = await _categoryService.GetAll();
						return View(rq);
					}
					for (int i = 0; i < images.Count; i++)
					{
						var productImage = new ProductImageCreateVM
						{
							Caption = rq.ProductName,
							ImageFile = images[i],
							IsDefault = true ? i == 0 : false,

						};

						var ig = await _productService.AddImage(productID, productImage);

					}
				}
				catch
				{
					ModelState.AddModelError(string.Empty, "Có lỗi xảy ra");
					return View(rq);
				}
				return RedirectToAction("Index");
			}
			ModelState.AddModelError(string.Empty, "Có lỗi xảy ra");
			rq.Categories = await _categoryService.GetAll();
			return View(rq);
		}

		// GET: ProductController/Edit/5
		[HttpGet]
		public async Task<ActionResult> Edit(int id)
		{
			var productVM = await _productService.GetById(id);
			var allCategory = await _categoryService.GetAll();
			var editProductVM = new ProductUpdateRequest
			{
				productVM = productVM,
				Categories = allCategory
			};
			return View(editProductVM);
		}

		// POST: ProductController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit(ProductUpdateRequest rq)
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

		// GET: ProductController/Delete/5
		public ActionResult Delete()
		{
			return View();
		}

		// POST: ProductController/Delete/5
		[HttpPost]
		public async Task<ActionResult> Delete(int id)
		{
			var product = await _productService.Delete(id);
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
