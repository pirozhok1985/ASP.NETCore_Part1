using WebStore.Domain.Entities.Orders;
using WebStore.Domain.ViewModels;

namespace WebStore.Domain.DTO;

public class OrderDto
{
    // public User User { get; set; }
    public int Id { get; set; }
    
    public string Phone { get; set; }
    
    public string Address { get; set; }
    
    public string? Description { get; set; }
    
    public DateTimeOffset Date { get; set; } = DateTimeOffset.Now;
    
    public ICollection<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
}

public static class OrderDtoMapper
{
    public static OrderDto ToDto(this Order order) => order is null
        ? null
        : new OrderDto
        {
            Address = order.Address,
            Date = order.Date,
            Description = order.Description,
            Id = order.Id,
            Items = new List<OrderItemDto>(order.Items.ToDto()),
            Phone = order.Phone,
        };
    public static Order FromDto(this OrderDto order) => order is null
        ? null
        : new Order
        {
            Address = order.Address,
            Date = order.Date,
            Description = order.Description,
            Id = order.Id,
            Items = new List<OrderItem>(order.Items.FromDto()),
            Phone = order.Phone,
        };
    public static IEnumerable<OrderDto> ToDto(this IEnumerable<Order> orders) => orders.Select(ToDto);
    public static IEnumerable<Order> FromDto(this IEnumerable<OrderDto> orders) => orders.Select(FromDto);

    public static IEnumerable<OrderItemDto> ToDto(this CartViewModel cart) => cart.Items.Select
        (i => new OrderItemDto
        {
            Price = i.product.Price,
            ProductId = i.product.Id,
            Quantity = i.Quantity
        });

    public static CartViewModel ToCartView(this IEnumerable<OrderItemDto> orderItems) => new CartViewModel
    {
        Items = orderItems.Select(p => (new ProductViewModel{Id = p.ProductId},p.Quantity))
    };
}