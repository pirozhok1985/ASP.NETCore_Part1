using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.DTO;
using WebStore.Domain.Entities.Orders;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Orders;

public class OrdersClient : BaseClient, IOrderService
{
    public OrdersClient(HttpClient client) : base(client, "api/orders")
    {
    }

    public async Task<IEnumerable<Order>> GetUserOrdersAsync(string userName, CancellationToken cancellationToken = default)
    {
        var result = await GetAsync<IEnumerable<OrderDto>>($"{Address}/user/{userName}");
        return result!.FromDto();
    }

    public async Task<Order?> GetOrderByIdAsync(int orderId, CancellationToken cancellationToken = default)
    {
        var result = await GetAsync<OrderDto>($"{Address}/{orderId}");
        return result!.FromDto();
    }

    public async Task<Order> CreateNewOrderAsync(string userName, CartViewModel cartViewModel, OrderViewModel orderViewModel,
        CancellationToken cancellationToken = default)
    {
        var model = new CreateOrderDto
        {
            Items = cartViewModel.ToDto(),
            Order = orderViewModel,
        };
        var respond = await PostAsync($"{Address}/{userName}", model);
        var result = await respond!
            .EnsureSuccessStatusCode()
            .Content.ReadFromJsonAsync<OrderDto>()
            .ConfigureAwait(false);
        return result!.FromDto();
    }
}