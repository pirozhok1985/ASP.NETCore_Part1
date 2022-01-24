using WebStore.Domain.Entities.Orders;
using WebStore.ViewModels;

namespace WebStore.Services.Interfaces;

public interface IOrderService
{
    Task<IEnumerable<Order>> GetUserOrdersAsync(string userName, CancellationToken cancellationToken = default);
    Task<Order?> GetOrderByIdAsync(int orderId, CancellationToken cancellationToken = default);
    Task<Order> CreateNewOrder(string userName, CartViewModel cartViewModel, OrderViewModel orderViewModel, CancellationToken cancellationToken = default);
}