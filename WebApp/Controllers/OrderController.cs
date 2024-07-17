using Application.Cart;
using Application.Order;
using Application.Payments;
using Application.Payments.VNPay;
using Application.System.Users;
using Data.EF.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq.Expressions;
using ViewModel.Cart;
using ViewModel.Order;
using ViewModel.Payment;
using ViewModel.Products;
using ViewModel.System;

namespace WebApp.Controllers
{
    public class OrderController : Controller
	{
		private readonly IShoppingCartService _cartService;
		private readonly IOrderService _orderService;
		private readonly IVNPayService _vnpayService;
		private readonly BaseHandleCheckout _handlePayment;
		public OrderController(IOrderService orderService,
								IShoppingCartService cartService,
								IVNPayService vnpayService,
                                BaseHandleCheckout handlePayment)
		{
			_orderService = orderService;
			_cartService = cartService;
			_vnpayService = vnpayService;
			_handlePayment = handlePayment;
		}
		public IActionResult Index()
		{

			return View();
		}

		[HttpGet]
		public async Task<IActionResult> CreateOrder(List<int> productIds)
		{
			if (productIds == null || !productIds.Any())
			{
				return RedirectToAction("Index", "Cart");
			}

			var cart = await _cartService.GetCart();
			var selectedItems = cart.Items.Where(item => productIds.Contains(item.ProductId)).ToList();

			//TempData["SelectedProductIds"] = JsonConvert.SerializeObject(selectedItems.Select(item => item.ProductId).ToList());

			TempData["selectedItems"] = JsonConvert.SerializeObject(selectedItems);
			if (!selectedItems.Any())
			{
				return RedirectToAction("Index", "Cart");
			}

			var orderVM = new OrderVM
			{
				OrderDate = DateTime.Now,
				OrderDetails = selectedItems.Select(item => new OrderDetailVM
				{
					ProductId = item.ProductId,
					Product = item.Product,
					Quantity = item.Quantity,
					Price = item.Product.SalePrice,

				}).ToList(),

			};
			orderVM.TotalAmount = orderVM.GetTotal();
			return View(orderVM);
		}

		[HttpPost]
		public async Task<IActionResult> CreateOrder(OrderVM orderVM)
		{
			var _selectedItemsJson = TempData["selectedItems"] as string;
			var _selectedItems = JsonConvert.DeserializeObject<List<CartItemVM>>(_selectedItemsJson);

			orderVM.OrderDetails = _selectedItems.Select(item => new OrderDetailVM
			{
				ProductId = item.ProductId,
				Product = item.Product,
				Quantity = item.Quantity,
				Price = item.Product.SalePrice,

			}).ToList();
			orderVM.TotalAmount = orderVM.GetTotal();

			switch (orderVM.PaymentMethod)
			{
				case PaymentMethod.COD:
					var result = await _orderService.CreateOrder(orderVM);

					if (result.IsSuccessed)
					{
						foreach (var item in _selectedItems)
						{
							await _cartService.RemoveFromCart(item.ProductId);
						}

						return RedirectToAction("OrderSuccess");
					}
					ModelState.AddModelError(string.Empty,"Có lỗi xảy ra, vui lòng thử lại sau");
					return View();
					
				case PaymentMethod.MOMO:
					break;
				case PaymentMethod.Paypal:
					break;
				case PaymentMethod.VNPay:
					//var resultHandle = _handlePayment.ProcessPayment(orderVM);
					break;
				default:
					// Xử lý trường hợp phương thức thanh toán không hợp lệ
					return BadRequest("Phương thức thanh toán không hợp lệ.");
			}
			return BadRequest("Có lỗi xảy ra");
		}

		public async Task<IActionResult> PaymentCallback()
		{
			var respone = _vnpayService.PaymentExcute(Request.Query);
			if (respone == null || respone.VnPayResponseCode != "00")
			{
				ModelState.AddModelError(string.Empty, $"Thanh toán thất bại, mã {respone.VnPayResponseCode}");
				return View();
			}

			return RedirectToAction("OrderSuccess");
		}

		public async Task<IActionResult> OrderSuccess()
		{
			return View();
		}
	}
}
