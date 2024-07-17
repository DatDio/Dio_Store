namespace Data.EF.Entities
{
	public class Cart
	{
		public Cart()
		{
			Products = new HashSet<Product>();
		}

		public int CartID { get; set; }
		public string CustomerId { get; set; }
		public double? TotalAmount { get; set; }
		public virtual Account Account { get; set; }
		public virtual ICollection<Product> Products { get; set; }
	}
}
