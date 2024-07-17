using System;
using System.Collections.Generic;

namespace Data.EF.Entities
{
	public class Product
	{
		public Product()
		{
			OrderDetails = new HashSet<OrderDetail>();
			Ratings = new HashSet<Rating>();

			ProductImages = new HashSet<ProductImages>();
		}

		public int ProductId { get; set; }
		public string ProductName { get; set; }
		public string Description { get; set; }
		public double SalePrice { get; set; }
		public double OriginalPrice { set; get; }
		public int? CartId { get; set; }
		public int? CategoryId { get; set; }
		public int? ViewCount { get; set; }
		public DateTime DateCreated { set; get; }
		public int? Stock { get; set; }
		public string? ThumbnailImage { get; set; }
		public virtual Category? Category { get; set; }
		public virtual Cart? Cart { get; set; }
		public virtual ICollection<OrderDetail> OrderDetails { get; set; }
		public virtual ICollection<Rating> Ratings { get; set; }

		public virtual ICollection<ProductImages> ProductImages { get; set; }
	}
}
