using ViewModel.Products;

namespace ViewModel.Order
{
	public class OrderDetailVM
	{
		public int Id { get; set; }
		public int OrderId { get; set; }
		public int ProductId { get; set; }
		public int Quantity { get; set; }
		public double Price { get; set; }
		public OrderVM Order { get; set; }
		public ProductVM Product { get; set; }
		public double SingleTotal()
		{
			return (Quantity * Price);
		}
	}
}
