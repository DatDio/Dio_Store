using Data.EF.Entities;
using ViewModel.Common;
using ViewModel.Order;

namespace Application.Order
{
    public interface IOrderService
    {
        Task<ResultModel<bool>> CreateOrder(OrderVM order);
		Task<ResultModel<bool>> UpdateStatusOrder(OrderVM order, string status);
		Task<PagedResult<OrderVM>> GetOrdersByUser(int pageIndex, int pageSize);
        Task<PagedResult<OrderVM>> GetAllOrder(int pageIndex, int pageSize);
    }
}
