namespace WebStore.Domain.ViewModels;

public class CartViewModel
{
    public IEnumerable<(ProductViewModel product,int Quantity)> Items { get; set; }
    public int ItemCount => Items.Sum(p => p.Quantity);
    public decimal TotalPrice => Items.Sum(p => p.product.Price * p.Quantity);
}