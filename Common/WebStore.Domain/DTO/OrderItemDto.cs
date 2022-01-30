using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Orders;

namespace WebStore.Domain.DTO;

public class OrderItemDto
{
    public int Id { get; set; }
    
    public int ProductId { get; set; }
    
    public decimal Price { get; set; }

    public int Quantity { get; set; }
}

public static class OrderItemsDtoMapper
{
    public static OrderItemDto ToDto(this OrderItem orderItem) => orderItem is null
        ? null
        : new OrderItemDto
        {
            Id = orderItem.Id,
            Price = orderItem.Price,
            Quantity = orderItem.Quantity,
            ProductId = orderItem.Product.Id,
        };
    public static OrderItem FromDto(this OrderItemDto orderItem) => orderItem is null
        ? null
        : new OrderItem
        {
            Id = orderItem.Id,
            Price = orderItem.Price,
            Quantity = orderItem.Quantity,
            Product = new Product{Id = orderItem.ProductId},
        };

    public static IEnumerable<OrderItemDto> ToDto(this IEnumerable<OrderItem> orderItems) => orderItems.Select(ToDto);
    public static IEnumerable<OrderItem> FromDto(this IEnumerable<OrderItemDto> orderItems) => orderItems.Select(FromDto);
}