using Application.Order;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using ViewModel.Order;

namespace Application.Payments
{
	public abstract class BaseHandleCheckout
	{
		private readonly IOrderService _orderService;
		public BaseHandleCheckout(IOrderService orderService)
		{
			_orderService= orderService;
		}
		protected virtual async Task<bool> SaveOrder(OrderVM order)
		{
			var result = await _orderService.CreateOrder(order);
			if(result.IsSuccessed)
			return true;
			return false;	
		}
		public abstract Task<IActionResult> CheckoutAsync(OrderVM order, HttpContext httpContext);
	}
}
