using Application.Cart;
using Application.Helper;
using Application.Product;
using Data.EF.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
	public class CartController : Controller
	{
		private readonly IShoppingCartService _shoppingCartService;
		private readonly IProductServicecs _productService;
		public CartController(IShoppingCartService shoppingCartService,
			IProductServicecs productService)
		{
			_shoppingCartService = shoppingCartService;
			_productService = productService;
		}
		public async Task<IActionResult> Index()
		{
			var cart = await _shoppingCartService.GetCart();
			return View(cart);
		}
		[HttpPost]
		public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
		{
			var product = await _productService.GetById(productId); // Implement this method to retrieve product
			if (product != null)
			{
				_shoppingCartService.AddToCart(product, quantity);
			}
            var cart = await _shoppingCartService.GetCart();
			if (cart != null)
			{
                
                return Json(new { success = true, message = "Thêm thành công!", cart });
			}
			return Json(new { success = false, message = "Có lỗi xảy ra!" });
		}

		[HttpPost]
		public async Task<IActionResult> UpdateCart(int productId, int quantity)
		{
			if (quantity <= 0)
			{
				return Json(new { success = false, message = "Số lượng phải lớn hơn 0!" });
			}

			await _shoppingCartService.UpdateCart(productId, quantity);
			var cart = await _shoppingCartService.GetCart();
			var singlePrice = cart.Items.FirstOrDefault(i => i.ProductId == productId)
									.GetSingleProductPrice();
			if (cart != null)
			{
				return Json(new { success = true, message = "Cập nhật thành công!", cart, singlePrice });
			}
			return Json(new { success = false, message = "Có lỗi xảy ra!" });
		}

		[HttpPost]
        public async Task<IActionResult> RemoveFromCart(int productId)
		{
			await _shoppingCartService.RemoveFromCart(productId);
			var cart = await _shoppingCartService.GetCart();
			if (cart != null)
			{
				return Json(new { success = true, message = "Xóa thành công!", cart });
			}
			return Json(new { success = false, message = "Có lỗi xảy ra!" });
		}
	}
}
