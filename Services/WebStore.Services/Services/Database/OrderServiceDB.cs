using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Orders;
using WebStore.Domain.Identity;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Services.Services.Database;

public class OrderServiceDB : IOrderService
{
    private readonly WebStoreDB _db;
    private readonly UserManager<User> _userManager;

    public OrderServiceDB(WebStoreDB db, UserManager<User> userManager)
    {
        _db = db;
        _userManager = userManager;
    }
    public async Task<IEnumerable<Order>> GetUserOrdersAsync(string userName, CancellationToken cancellationToken = default)
    {
         return await _db.Orders
            .Include(o => o.User)
            .Include(o => o.Items)
            // .ThenInclude(i => i.Order)
            .Where(o => o.User.UserName == userName)
            .ToArrayAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<Order?> GetOrderByIdAsync(int orderId, CancellationToken cancellationToken = default)
    {
        return await _db.Orders
            .Include(o => o.User)
            .Include(o => o.Items)
            .ThenInclude(i => i.Order)
            .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<Order> CreateNewOrder(string userName, CartViewModel cartViewModel, OrderViewModel orderViewModel,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByNameAsync(userName).ConfigureAwait(false);
        if (user is null)
            throw new InvalidOperationException($"Пользователь {userName} в БД не найден");

        await using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);   
            
        var order = new Order
        {
            Address = orderViewModel.Address,
            User = user,
            Phone = orderViewModel.Phone,
            Description = orderViewModel.Description,
        };
        var productIds = cartViewModel.Items.Select(i => i.product.Id).ToArray();
        var products = await _db.Products.Where(p => productIds.Contains(p.Id))
            .ToArrayAsync(cancellationToken)
            .ConfigureAwait(false);
        order.Items = cartViewModel.Items.Join(
            products,
            cartItem => cartItem.product.Id,
            cartProduct => cartProduct.Id,
            (cartItem,cartProduct) => new OrderItem
            {
                Order = order,
                Product = cartProduct,
                Price = cartProduct.Price,
                Quantity = cartItem.Quantity
            }
        ).ToArray();
        await _db.AddAsync(order, cancellationToken).ConfigureAwait(false);
        await _db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        await transaction.CommitAsync(cancellationToken);
        return order;
    }
}