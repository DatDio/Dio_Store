using Application.Extensions;
using Data.EF.Entities;
using ViewModel.Cart;
using ViewModel.Products;
using ViewModel.System;

namespace Application.Cart
{
	public class ShoppingCartService : IShoppingCartService
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public ShoppingCartService(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}
		private ISession Session => _httpContextAccessor.HttpContext.Session;

		public async Task<ShoppingCartVM> GetCart()
		{
			try
			{
                var cart = Session.GetObjectFromJson<ShoppingCartVM>(SystemContants.CartSession) ?? new ShoppingCartVM();
                return cart;
            }
			catch
			{
			}
            return null;
        }

		public async Task AddToCart(ProductVM product, int quantity)
		{
			var cart = await GetCart();
			cart.AddToCart(product, quantity);
			Session.SetObjectAsJson(SystemContants.CartSession, cart);
		}

		public async Task RemoveFromCart(int productId)
		{
			var cart = await GetCart();
			cart.RemoveFromCart(productId);
			Session.SetObjectAsJson(SystemContants.CartSession, cart);
		}
		public async Task UpdateCart(int productId, int quantity)
		{
			var cart = await GetCart();
			cart.UpdateCart(productId, quantity);
			Session.SetObjectAsJson(SystemContants.CartSession, cart);
		}

		public async Task<double> GetTotal()
		{
			var cart = await GetCart();
			return cart.GetSumTotal();
		}
	}
}
