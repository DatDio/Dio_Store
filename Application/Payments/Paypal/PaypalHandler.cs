using Application.Order;
using Microsoft.AspNetCore.Mvc;
using ViewModel.Order;

namespace Application.Payments.Paypal
{
	public class PaypalHandler : BaseHandleCheckout
	{
		public PaypalHandler(IOrderService orderService) : base(orderService)
		{

		}
		public override Task<IActionResult> CheckoutAsync(OrderVM order, HttpContext httpContext)
		{
			throw new NotImplementedException();
		}
	}
}
