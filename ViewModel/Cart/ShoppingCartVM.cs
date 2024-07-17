using ViewModel.Products;
namespace ViewModel.Cart
{
	public class ShoppingCartVM
	{
		public List<CartItemVM> Items { get; set; } = new List<CartItemVM>();

		public void AddToCart(ProductVM product, int quantity)
		{
			var existingItem = Items.FirstOrDefault(i => i.ProductId == product.Id);
			if (existingItem != null)
			{
				existingItem.Quantity += quantity;
			}
			else
			{
				Items.Add(new CartItemVM { 
					ProductId = product.Id,
					Product = product,
					Quantity = quantity });
			}
		}

		public void RemoveFromCart(int productId)
		{
			var item = Items.FirstOrDefault(i => i.ProductId == productId);
			if (item != null)
			{
				Items.Remove(item);
			}
		}
		public void UpdateCart(int productId, int quantity)
		{
			var item = Items.FirstOrDefault(i => i.ProductId == productId);
			if (item != null)
			{
				item.Quantity = quantity;
			}
		}
		public double GetSumTotal()
		{
			return (double)Items.Sum(i => i.Product.SalePrice * i.Quantity);
		}
	}

}
