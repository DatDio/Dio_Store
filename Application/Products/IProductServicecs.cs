using ViewModel.Common;
using ViewModel.ProductImages;
using ViewModel.Products;

namespace Application.Product
{
	public interface IProductServicecs
	{
		Task<int> Create(ProductCreateVM request);

		Task<int> Update(ProductUpdateRequest request);

		Task<int> Delete(int productId);

		Task<ProductVM> GetById(int productId);

		Task<bool> UpdatePrice(int productId, double newPrice);

		Task<bool> UpdateStock(int productId, int addedQuantity);

		Task AddViewcount(int productId);

		//Task<PageResult<ProductVM>> GetAllPaging(GetManageProductPagingRequest request);

		Task<int> AddImage(int productId, ProductImageCreateVM request);

		Task<int> RemoveImage(int imageId);

		Task<int> UpdateImage(int imageId, ProductImageUpdateVm request);

		Task<ProductImageVM> GetImageById(int imageId);

		Task<List<ProductImageVM>> GetListImages(int productId);

		//Task<PageResult<ProductVM>> GetAllByCategoryId(string languageId, GetPublicProductPagingRequest request);

		//Task<ApiResult<bool>> CategoryAssign(int id, CategoryAssignRequest request);

		Task<PagedResult<ProductVM>> GetPagingProducts(int pageIndex, int pageSize);

		Task<PagedResult<ProductVM>> GetPagingProductsByKeyword(int pageIndex, int pageSize, string keyWord);

		Task<List<ProductVM>> GetLatestProducts(int take);
		Task<List<ProductVM>> GetBestSellingProducts(int take);
	}
}
