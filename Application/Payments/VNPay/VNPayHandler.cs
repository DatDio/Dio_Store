using Application.Order;
using Data.EF.Entities;
using Microsoft.AspNetCore.Mvc;
using ViewModel.Order;
using ViewModel.Payment;

namespace Application.Payments.VNPay
{
	public class VNPayHandler : BaseHandleCheckout
	{

		private readonly IVNPayService _vnpayService;
		public VNPayHandler(IOrderService orderService, IVNPayService vnpayService)
		: base(orderService) 
		{
			_vnpayService = vnpayService;
		}
		public override async Task<IActionResult> CheckoutAsync(OrderVM orderVM, HttpContext httpContext)
		{
			// Logic for VNPay payment
			var vnPayRequest = new VnPaymentRequestModel
			{
				Amount = orderVM.TotalAmount,
				CreatedDate = DateTime.Now,
				Description = $"{orderVM.ShipName} {orderVM.ShipPhoneNumber}",
				Fullname = orderVM.ShipName,
				OrderId = Guid.NewGuid().ToString()
			};
			var redirect = _vnpayService.CreatePaymentUrl(httpContext, vnPayRequest);
			return new RedirectResult(redirect);
		}
		public async Task<IActionResult> HandleCallBack(OrderVM orderVM, HttpContext httpContext)
		{
			var queryCollection = httpContext.Request.Query;
			var response = _vnpayService.PaymentExcute(queryCollection);
			if (response == null || response.VnPayResponseCode != "00")
			{
				return new BadRequestObjectResult($"Thanh toán thất bại, mã {response.VnPayResponseCode}");
			}

			if (await SaveOrder(orderVM))
			{
				return new RedirectResult("Order/OrderSuccess");
			}
			return new BadRequestObjectResult($"Có lỗi xảy ra, vui lòng thử lại sau");
		}
	}
}
