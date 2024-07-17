using System.ComponentModel.DataAnnotations;
using ViewModel.Category;

namespace ViewModel.Products
{
    public class ProductCreateVM
    {
		[Required(ErrorMessage = "Vui lòng nhập tên sản phẩm!")]
		public string ProductName { get; set; }
		[Required(ErrorMessage = "Vui lòng thông tin sản phẩm!")]
		public string? Description { get; set; }
		[Required(ErrorMessage = "Vui lòng giá sản phẩm!")]
		public double? OriginalPrice { get; set; }
        public double? SalePrice { get; set; }
		public int? Stock { get; set; }
		public int? CategoryId { get; set; }
		public List<CategoryVM>? Categories { get; set; }
		//public string? ImagePath { get; set; }

		//public string? Caption { get; set; }

		//public bool IsDefault { get; set; }
		//public IFormFile ThumbnailImage { get; set; }
	}
}
