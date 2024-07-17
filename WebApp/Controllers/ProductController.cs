using Application.Categories;
using Application.Product;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductServicecs _productService;
        private readonly ICatetgoryServicecs _categoryService;
        public ProductController(IProductServicecs productService, ICatetgoryServicecs catetgoryServicecs)
        {
            _productService = productService;
            _categoryService = catetgoryServicecs;

        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Detail(int productID)
        {
            var product = await _productService.GetById(productID);
            await _productService.AddViewcount(productID);
            return View(product);
        }
    }
}
