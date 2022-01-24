namespace WebStore.Domain.ViewModels;

public class CartOrderViewModel
{
    public CartViewModel CartViewModel { get; set; } = null;
    public OrderViewModel OrderViewModel { get; set; } = new();
}