using Application.Cart;
using Data.EF.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ViewModel.Users;

namespace WebApp.Components
{
	public class CartViewComponent : ViewComponent
	{
		private readonly IShoppingCartService _shoppingCartService;
		public CartViewComponent(IShoppingCartService shoppingCartService)
		{
			_shoppingCartService = shoppingCartService;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var cart = await _shoppingCartService.GetCart();
			return View(cart);	
		}
	}
}
