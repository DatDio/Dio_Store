namespace ViewModel.Order
{
	public class OrderVM
	{
		public int Id { get; set; }
		public string UserId { get; set; }
		public DateTime OrderDate { get; set; }
		public double TotalAmount { get; set; }
		public string ShipName { set; get; }
		public string ShipAddress { set; get; }
        public string PaymentMethod { set; get; }
        public string OrderStatus { get; set; }
		public string ShipEmail { set; get; }
		public string ShipPhoneNumber { set; get; }
		public ICollection<OrderDetailVM> OrderDetails { get; set; }
		public double GetTotal()
		{
			return TotalAmount = (double)OrderDetails.Sum(i => i.Product.SalePrice * i.Quantity);
		}
	}
}
