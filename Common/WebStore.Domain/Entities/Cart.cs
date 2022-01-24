namespace WebStore.Domain.Entities;

public class Cart
{
    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    public int ItemsSum => CartItems.Sum(c => c.Quantity);
}