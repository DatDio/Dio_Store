using Data.EF;
using Data.EF.Entities;
using Microsoft.EntityFrameworkCore;
using ViewModel.Category;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Application.Categories
{
	public class CategoryService : ICatetgoryServicecs
	{
		private readonly Dio_StoreContext _context;
		public CategoryService(Dio_StoreContext context)
		{
			_context = context;
		}
		public async Task<List<CategoryVM>> GetAll()
		{
			return await _context.Categories
		.Select(x => new CategoryVM()
		{
			Id = x.CategoryId,
			Name = x.CategoryName,
		})
		.ToListAsync();
		}

		public async Task<CategoryVM> GetById(int id)
		{
			var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
			if(category != null )
			{
				return new CategoryVM()
				{
					Id = category.CategoryId,
					Name = category.CategoryName,
				};
			}
			return null;
		}
	}
}
