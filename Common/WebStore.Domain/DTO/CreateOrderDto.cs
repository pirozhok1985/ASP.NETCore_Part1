using WebStore.Domain.ViewModels;

namespace WebStore.Domain.DTO;

public class CreateOrderDto
{
    public OrderViewModel? Order { get; set; }
    public IEnumerable<OrderItemDto>? Items { get; set; }
}