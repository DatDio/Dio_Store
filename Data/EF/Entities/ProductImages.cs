namespace Data.EF.Entities
{
	public class ProductImages
	{
		public int ProductImageId { get; set; }

		public int ProductId { get; set; }

		public string? ImagePath { get; set; }

		public string? Caption { get; set; }

		public bool IsDefault { get; set; }

		public virtual Product? Product { get; set; }
	}
}
