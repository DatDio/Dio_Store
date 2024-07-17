using ViewModel.Cart;
using ViewModel.Products;

namespace Application.Cart
{
	public interface IShoppingCartService
	{
		Task<ShoppingCartVM> GetCart();
		Task AddToCart(ProductVM product, int quantity);
		Task RemoveFromCart(int productId);
		Task UpdateCart(int productId, int quantity);
		Task<double> GetTotal();
	}
}
