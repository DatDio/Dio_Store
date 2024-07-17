using ViewModel.Products;

namespace ViewModel.Cart
{
	public class CartItemVM
	{
		public int ProductId { get; set; }
		public ProductVM Product { get; set; }
		public int Quantity { get; set; }
		public double GetSingleProductPrice()
		{
			return (double)(Product.SalePrice * Quantity);
		}
	}
}
