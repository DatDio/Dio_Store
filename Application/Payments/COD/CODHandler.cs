using Application.Order;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ViewModel.Order;

namespace Application.Payments.COD
{
	public class CODHandler : BaseHandleCheckout
	{
		public CODHandler(IOrderService orderService) : base(orderService)
		{

		}
		public override async Task<IActionResult> CheckoutAsync(OrderVM order, HttpContext httpContext)
		{
			if (await SaveOrder(order))
			{
				return new RedirectResult("Order/OrderSuccess");
			}
			return new BadRequestObjectResult($"Có lỗi xảy ra, vui lòng thử lại sau");
		}

	}
}
