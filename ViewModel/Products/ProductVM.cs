using ViewModel.Category;
using ViewModel.ProductImages;

namespace ViewModel.Products
{
    public class ProductVM
    {
        public int Id { set; get; }
        public string ProductName { set; get; }
        public double SalePrice { set; get; }
        public double? OriginalPrice { set; get; }
        public int? Stock { set; get; }
        public int? ViewCount { set; get; }
        public DateTime DateCreated { set; get; }
        public string Description { set; get; }
        //public string ThumbnailImage { get; set; }
        public  List<ProductImageVM>? ProductImageVM { get; set; }
		public List<CategoryVM>? CategoryVM { get; set; }
    }
}
