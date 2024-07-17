using Data.EF.Entities;
using ViewModel.Category;

namespace Application.Categories
{
	public interface ICatetgoryServicecs
	{
		Task<List<CategoryVM>> GetAll();

		Task<CategoryVM> GetById(int id);
	}
}
