using Application.Files;
using Application.Product;
using Data.EF;
using Data.EF.Entities;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using System.Linq;
using System.Net.Http.Headers;
using ViewModel.Category;
using ViewModel.Common;
using ViewModel.ProductImages;
using ViewModel.Products;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Application.Product
{
	public class ProductService : IProductServicecs
	{
		private readonly Dio_StoreContext _context;
		private readonly IStorageService _storageService;
		private const string FOLDER_NAME = "Images/ProductImages";
		public ProductService(Dio_StoreContext context, IStorageService storageService)
		{
			_context = context;
			_storageService = storageService;

		}

		public async Task<int> AddImage(int productId, ProductImageCreateVM request)
		{
			try
			{
				var productImage = new ProductImages()
				{
					Caption = request.Caption,
					//DateCreated = DateTime.Now,
					IsDefault = request.IsDefault,
					ProductId = productId,
					//SortOrder = request.SortOrder
				};

				if (request.ImageFile != null)
				{
					productImage.ImagePath = await this.SaveFile(request.ImageFile);
					//productImage.FileSize = request.ImageFile.Length;
				}
				_context.ProductImages.Add(productImage);
				await _context.SaveChangesAsync();
				return productImage.ProductImageId;
			}
			catch
			{

			}
			return 0;
		}

		public async Task<int> UpdateImage(int imageId, ProductImageUpdateVm request)
		{
			var productImage = await _context.ProductImages.FindAsync(imageId);
			if (productImage == null)
				return 0;

			if (request.ImageFile != null)
			{
				productImage.ImagePath = await this.SaveFile(request.ImageFile);
				//productImage.FileSize = request.ImageFile.Length;
			}
			_context.ProductImages.Update(productImage);
			return await _context.SaveChangesAsync();
		}

		public async Task AddViewcount(int productId)
		{
			var product = await _context.Products.FindAsync(productId);
			if (product != null)
			{
				product.ViewCount += 1;
				await _context.SaveChangesAsync();
			}
		}

		public async Task<int> Create(ProductCreateVM request)
		{
			try
			{
				var product = new Data.EF.Entities.Product()
				{
					SalePrice = (double)request.SalePrice,
					OriginalPrice = (double)request.OriginalPrice,
					ProductName = request.ProductName,
					Description = request.Description,
					Stock = request.Stock,
					CategoryId = request.CategoryId,
				};

				//Save image

				//if (request.ThumbnailImage != null)
				//{
				//	product.ProductImages = new List<ProductImages>()
				//	 {
				//		 new ProductImages()
				//		 {
				//			 Caption = "Thumbnail image",
				//			 ImagePath = await this.SaveFile(request.ThumbnailImage),
				//			 IsDefault = true,
				//		 }
				//	 };
				//}

				_context.Products.Add(product);
				try
				{
					await _context.SaveChangesAsync();
				}
				catch
				{

				}


				return product.ProductId;
			}
			catch (Exception ex)
			{
				// Log the exception (e.g., using a logging framework)
				Console.WriteLine(ex.Message);
				throw;
			}
		}

		public async Task<int> Delete(int productId)
		{
			try
			{
				var product = await _context.Products.FindAsync(productId);
				if (product == null) throw new Exception("Không tìm thấy sản phẩm!");

				var images = await _context.ProductImages.Where(i => i.ProductId == productId).ToListAsync();
				foreach (var image in images)
				{
					await _storageService.DeleteFileAsync(image.ImagePath);
					_context.ProductImages.Remove(image);
				}

				_context.Products.Remove(product);
				return await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{

				return 0;
			}
		}


		public Task<PagedResult<ProductVM>> GetAllPaging(string keyWord, int pageIndex, int pageSize)
		{
			throw new NotImplementedException();
		}

		public async Task<ProductVM> GetById(int productId)
		{
			var product = await _context.Products.FindAsync(productId);
			if (product == null)
			{
				return null; // hoặc xử lý khác nếu sản phẩm không tồn tại
			}

			var categories = await _context.Categories
				.Where(c => c.Products.Any(p => p.ProductId == productId))
				.ToListAsync();

			var productImages = await _context.ProductImages
				.Where(x => x.ProductId == productId)
				.ToListAsync();

			var productViewModel = new ProductVM()
			{
				Id = product.ProductId,
				Description = product.Description,
				ProductName = product.ProductName,
				OriginalPrice = (double)product.OriginalPrice,
				SalePrice = (double)product.SalePrice,
				Stock = product.Stock,
				DateCreated = product.DateCreated,
				ViewCount = product.ViewCount,
				CategoryVM = categories.Select(c => new CategoryVM
				{
					Id = c.CategoryId,
					Name = c.CategoryName
					// Thêm các thuộc tính khác nếu cần
				}).ToList(),
				ProductImageVM = productImages
					.Select(pi => new ProductImageVM
					{
						Caption = pi.Caption,
						IsDefault = pi.IsDefault,
						ImagePath = pi.ImagePath
					})
					.ToList()
			};

			return productViewModel;
		}
		public Task<ProductImageVM> GetImageById(int imageId)
		{
			throw new NotImplementedException();
		}

		public Task<List<ProductImageVM>> GetListImages(int productId)
		{
			throw new NotImplementedException();
		}

		public async Task<int> RemoveImage(int imageId)
		{
			var productImage = await _context.ProductImages.FindAsync(imageId);
			if (productImage == null)
				throw new Exception($"Cannot find an image with id {imageId}");
			_context.ProductImages.Remove(productImage);
			return await _context.SaveChangesAsync();
		}

		public Task<int> Update(ProductUpdateRequest request)
		{
			throw new NotImplementedException();
		}

		public async Task<bool> UpdatePrice(int productId, double newPrice)
		{
			var product = await _context.Products.FindAsync(productId);
			if (product == null) throw new Exception($"Cannot find a product with id: {productId}");
			product.SalePrice = newPrice;
			return await _context.SaveChangesAsync() > 0;
		}

		public async Task<bool> UpdateStock(int productId, int addedQuantity)
		{
			var product = await _context.Products.FindAsync(productId);
			if (product == null) throw new Exception($"Cannot find a product with id: {productId}");
			//product.Stock += addedQuantity;
			return await _context.SaveChangesAsync() > 0;
		}
		private async Task<string> SaveFile(IFormFile file)
		{
			var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
			var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
			await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
			//return "/" + FOLDER_NAME + "/" + fileName;
			return fileName;
		}

		public async Task<PagedResult<ProductVM>> GetPagingProducts(int pageIndex, int pageSize)
		{
			// Bước 1: Truy vấn các sản phẩm trước.
			var productsQuery = _context.Products
				.Skip((pageIndex - 1) * pageSize)
				.Take(pageSize);

			var products = await productsQuery.ToListAsync();

			// Lấy danh sách ProductIds từ các sản phẩm đã truy vấn.
			var productIds = products.Select(p => p.ProductId).ToList();

			// Bước 2: Truy vấn các hình ảnh tương ứng với các sản phẩm đã truy vấn.
			var productImages = await _context.ProductImages
				.Where(pi => productIds.Contains(pi.ProductId))
				.ToListAsync();

			// Bước 3: Kết hợp các kết quả lại với nhau trên client-side.
			var productVMs = products.Select(p => new ProductVM()
			{
				Id = p.ProductId,
				ProductName = p.ProductName,
				DateCreated = p.DateCreated,
				Description = p.Description,
				OriginalPrice = p.OriginalPrice,
				SalePrice = (double)p.SalePrice,
				Stock = p.Stock,
				ViewCount = p.ViewCount,
				ProductImageVM = productImages
					.Where(pi => pi.ProductId == p.ProductId)
					.Select(pi => new ProductImageVM
					{
						Caption = pi.Caption,
						IsDefault = pi.IsDefault,
						ImagePath = pi.ImagePath
					})
					.ToList()
			}).ToList();

			// Tổng số record
			int totalRow = await _context.Products.CountAsync();

			var pagedResult = new PagedResult<ProductVM>()
			{
				TotalRecords = totalRow,
				PageIndex = pageIndex,
				PageSize = pageSize,
				Items = productVMs
			};

			return pagedResult;
		}

		public async Task<List<ProductVM>> GetLatestProducts(int take)
		{
			var products = await _context.Products
							  .OrderByDescending(p => p.DateCreated)
							  .Take(take)
							  .Select(p => new ProductVM
							  {
								  Id = p.ProductId,
								  OriginalPrice = p.OriginalPrice,
								  SalePrice = (double)p.SalePrice,
								  Description = p.Description,
								  Stock = p.Stock,
								  DateCreated = p.DateCreated,
								  ViewCount = p.ViewCount,
								  ProductImageVM = _context.ProductImages
												   .Where(pi => pi.ProductId == p.ProductId)
												   .Select(pi => new ProductImageVM
												   {
													   //ProductId = pi.Id,
													   ProductId = pi.ProductId,
													   ImagePath = pi.ImagePath,
													   IsDefault = pi.IsDefault
												   }).ToList()
							  })
							  .ToListAsync();
			return products;
		}

		public Task<List<ProductVM>> GetBestSellingProducts(int take)
		{
			throw new NotImplementedException();
		}

		public async Task<PagedResult<ProductVM>> GetPagingProductsByKeyword(int pageIndex, int pageSize, string keyWord)
		{
			var products = await _context.Products.Where(p => p.ProductName.Contains(keyWord)
										|| p.Category.CategoryName.Contains(keyWord)).ToListAsync();
			var productIds = products.Select(p => p.ProductId).ToList();

			// Bước 2: Truy vấn các hình ảnh tương ứng với các sản phẩm đã truy vấn.
			var productImages = await _context.ProductImages
				.Where(pi => productIds.Contains(pi.ProductId))
				.ToListAsync();

			// Bước 3: Kết hợp các kết quả lại với nhau trên client-side.
			var productVMs = products.Select(p => new ProductVM()
			{
				Id = p.ProductId,
				ProductName = p.ProductName,
				DateCreated = p.DateCreated,
				Description = p.Description,
				OriginalPrice = p.OriginalPrice,
				SalePrice = (double)p.SalePrice,
				Stock = p.Stock,
				ViewCount = p.ViewCount,
				ProductImageVM = productImages
					.Where(pi => pi.ProductId == p.ProductId)
					.Select(pi => new ProductImageVM
					{
						Caption = pi.Caption,
						IsDefault = pi.IsDefault,
						ImagePath = pi.ImagePath
					})
					.ToList()
			}).ToList();

			// Tổng số record
			int totalRow = await _context.Products.CountAsync();

			var pagedResult = new PagedResult<ProductVM>()
			{
				TotalRecords = totalRow,
				PageIndex = pageIndex,
				PageSize = pageSize,
				Items = productVMs
			};
			return pagedResult;

		}
	}
}
