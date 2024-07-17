using Microsoft.AspNetCore.Mvc.RazorPages;
using ViewModel.Common;
using ViewModel.Products;

namespace WebApp.Models
{
	public class HomeViewModel
	{
		public List<ProductVM> LatestProducts { get; set; }
		public PagedResult<ProductVM> AllProduct { get; set; }
	}
}
