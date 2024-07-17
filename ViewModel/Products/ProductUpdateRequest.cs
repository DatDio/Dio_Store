using ViewModel.Category;

namespace ViewModel.Products
{
	public class ProductUpdateRequest
	{
		public ProductVM productVM { get; set; }
		public List<CategoryVM> Categories { get; set; }
	}
}
