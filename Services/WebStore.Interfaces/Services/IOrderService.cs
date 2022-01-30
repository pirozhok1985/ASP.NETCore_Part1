using WebStore.Domain.Entities.Orders;
using WebStore.Domain.ViewModels;

namespace WebStore.Interfaces.Services;

public interface IOrderService
{
    Task<IEnumerable<Order>> GetUserOrdersAsync(string userName, CancellationToken cancellationToken = default);
    Task<Order?> GetOrderByIdAsync(int orderId, CancellationToken cancellationToken = default);
    Task<Order> CreateNewOrderAsync(string userName, CartViewModel cartViewModel, OrderViewModel orderViewModel, CancellationToken cancellationToken = default);
}