namespace ViewModel.ProductImages
{
    public class ProductImageCreateVM
    {
        public string Caption { get; set; }

        public bool IsDefault { get; set; }

        //public int SortOrder { get; set; }

        public IFormFile ImageFile { get; set; }
    }
}
