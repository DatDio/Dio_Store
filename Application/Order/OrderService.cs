using Application.Cart;
using Application.Payments.VNPay;
using Application.System.Users;
using Data.EF;
using Data.EF.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;
using ViewModel.Common;
using ViewModel.Order;
using ViewModel.ProductImages;
using ViewModel.Products;
using ViewModel.System;

namespace Application.Order
{
	public class OrderService : IOrderService
	{
		private readonly Dio_StoreContext _context;
		private readonly IShoppingCartService _cartService;
		private readonly IUserService _userService;
		private readonly IVNPayService _vnpayService;
		public OrderService(Dio_StoreContext context,
			IShoppingCartService cartService,
			IUserService userService,
			IVNPayService vnpayService)
		{
			_context = context;
			_cartService = cartService;
			_userService = userService;
			_vnpayService = vnpayService;

		}
		public async Task<ResultModel<bool>> CreateOrder(OrderVM rq)
		{
			var order = new Data.EF.Entities.Order();
			{
				
				order.AccountID = await _userService.GetUserID();
				order.ShipPhoneNumber = rq.ShipPhoneNumber;
				order.ShipName = rq.ShipName;
				order.ShipAddress = rq.ShipAddress;
				order.ShipEmail = rq.ShipEmail;
				order.OrderDate = DateTime.Now;
				order.OrderStatus = OrderStatus.Status_Pending;
				order.TotalAmount = rq.TotalAmount;
				order.PaymentMethod = rq.PaymentMethod;
				order.OrderDetails = rq.OrderDetails.Select(od => new OrderDetail
				{
					ProductId = od.ProductId,
					Quantity = od.Quantity,
					OrderId = od.OrderId,
					Price = od.Price
				}).ToList();

			};
			_context.Orders.Add(order);

			if (await _context.SaveChangesAsync() > 0)
			{
				return new ResultModel<bool>
				{
					IsSuccessed = true,
				};
			}
			return new ResultModel<bool>
			{
				IsSuccessed = false,
				Message = "Có lỗi xảy ra!"
			};
		}

		public async Task<PagedResult<OrderVM>> GetAllOrder(int pageIndex, int pageSize)
		{
			// Bước 1: Truy vấn các đơn hàng trước.
			var orderQuery = _context.Orders
				.Skip((pageIndex - 1) * pageSize)
				.Take(pageSize);

			var orders = await orderQuery.ToListAsync();

			// Lấy danh sách OrderIds từ các đơn hàng đã truy vấn.
			var orderIds = orders.Select(o => o.OrderId).ToList();

			// Bước 2: Truy vấn các chi tiết đơn hàng tương ứng với các đơn hàng đã truy vấn.
			var orderDetails = await _context.OrderDetails
				.Where(od => orderIds.Contains(od.OrderId))
				.ToListAsync();

			// Truy vấn sản phẩm tương ứng với các chi tiết đơn hàng.
			var productIds = orderDetails.Select(od => od.ProductId).Distinct().ToList();

			var productImages = await _context.ProductImages
			  .Where(pi => productIds.Contains(pi.ProductId))
			  .ToListAsync();

			var products = await _context.Products
				.Where(p => productIds.Contains(p.ProductId))
				.Select(p => new ProductVM
				{
					Id = p.ProductId,
					ProductName = p.ProductName,
					SalePrice = p.SalePrice,
					ProductImageVM = productImages
					.Where(pi => pi.ProductId == p.ProductId)
					.Select(pi => new ProductImageVM
					{
						Caption = pi.Caption,
						IsDefault = pi.IsDefault,
						ImagePath = pi.ImagePath
					})
					.ToList()
				})
				.ToListAsync();


			// Bước 3: Kết hợp các kết quả lại với nhau.
			var orderVMs = orders.Select(o => new OrderVM()
			{
				Id = o.OrderId,
				OrderDate = o.OrderDate,
				OrderStatus = o.OrderStatus,
				PaymentMethod = o.PaymentMethod,
				ShipAddress = o.ShipAddress,
				ShipEmail = o.ShipAddress,
				ShipName = o.ShipName,
				ShipPhoneNumber = o.ShipPhoneNumber,
				TotalAmount = o.TotalAmount,
				OrderDetails = orderDetails
					.Where(od => od.OrderId == o.OrderId)
					.Select(od => new OrderDetailVM
					{
						OrderId = od.OrderId,
						Price = od.Price,
						ProductId = od.ProductId,
						Product = products.FirstOrDefault(p => p.Id == od.ProductId),

						Quantity = od.Quantity
					})
					.ToList()
			}).ToList();

			// Tổng số record
			int totalRow = await _context.Orders.CountAsync();

			var pagedResult = new PagedResult<OrderVM>()
			{
				TotalRecords = totalRow,
				PageIndex = pageIndex,
				PageSize = pageSize,
				Items = orderVMs
			};

			return pagedResult;
		}


		public async Task<PagedResult<OrderVM>> GetOrdersByUser(int pageIndex, int pageSize)
		{
			try
			{
				var userId = await _userService.GetUserID();

				// Bước 1: Truy vấn các đơn hàng của người dùng với phân trang.
				var orders = await _context.Orders
					.Where(o => o.AccountID == userId)
					.Skip((pageIndex - 1) * pageSize)
					.Take(pageSize)
					.ToListAsync();

				// Lấy danh sách OrderIds từ các đơn hàng đã truy vấn.
				var orderIds = orders.Select(o => o.OrderId).ToList();

				// Bước 2: Truy vấn các chi tiết đơn hàng tương ứng với các đơn hàng đã truy vấn.
				var orderDetails = await _context.OrderDetails
					.Where(od => orderIds.Contains(od.OrderId))
					.ToListAsync();

				// Truy vấn sản phẩm tương ứng với các chi tiết đơn hàng.
				var productIds = orderDetails.Select(od => od.ProductId).Distinct().ToList();

				var products = await _context.Products
					.Where(p => productIds.Contains(p.ProductId))
					.ToListAsync();

				var productImages = await _context.ProductImages
					.Where(pi => productIds.Contains(pi.ProductId))
					.ToListAsync();

				// Bước 3: Kết hợp các kết quả lại với nhau.
				var productVMs = products.Select(p => new ProductVM
				{
					Id = p.ProductId,
					ProductName = p.ProductName,
					SalePrice = p.SalePrice,
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

				var orderVMs = orders.Select(o => new OrderVM()
				{
					Id = o.OrderId,
					OrderDate = o.OrderDate,
					OrderStatus = o.OrderStatus,
					PaymentMethod = o.PaymentMethod,
					ShipAddress = o.ShipAddress,
					ShipEmail = o.ShipAddress,
					ShipName = o.ShipName,
					ShipPhoneNumber = o.ShipPhoneNumber,
					TotalAmount = o.TotalAmount,
					OrderDetails = orderDetails
						.Where(od => od.OrderId == o.OrderId)
						.Select(od => new OrderDetailVM
						{
							OrderId = od.OrderId,
							Price = od.Price,
							ProductId = od.ProductId,
							Product = productVMs.FirstOrDefault(p => p.Id == od.ProductId),
							Quantity = od.Quantity
						})
						.ToList()
				}).ToList();

				// Tổng số record đơn hàng của người dùng
				int totalRow = await _context.Orders.CountAsync(o => o.AccountID == userId);

				var pagedResult = new PagedResult<OrderVM>()
				{
					TotalRecords = totalRow,
					PageIndex = pageIndex,
					PageSize = pageSize,
					Items = orderVMs
				};

				return pagedResult;
			}
			catch
			{
				throw new Exception("Có lỗi xảy ra");
			}
		}


		public async Task<ResultModel<bool>> UpdateStatusOrder(OrderVM rq, string status)
		{
			var order = new Data.EF.Entities.Order();
			{
				//order.OrderId = rq.Id;
				order.OrderDate = rq.OrderDate;
				order.OrderStatus = status;
			};
			//_context.Orders.Add(order);

			if (await _context.SaveChangesAsync() > 0)
			{
				return new ResultModel<bool>
				{
					IsSuccessed = true,
				};
			}
			return new ResultModel<bool>
			{
				IsSuccessed = false,
				Message = "Có lỗi xảy ra!"
			};
		}
	}
}
