using Application.Categories;
using Data.EF.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ViewModel.Users;

namespace WebApp.Components
{
	public class HeaderBottomViewComponent : ViewComponent
	{
		private readonly ICatetgoryServicecs _catetgoryServicecs;

		public HeaderBottomViewComponent(ICatetgoryServicecs catetgoryServicecs)
		{
            _catetgoryServicecs = catetgoryServicecs;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var allCate = await _catetgoryServicecs.GetAll();
            return View(allCate);
		}
	}
}
