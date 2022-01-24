using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebStore.Domain.Entities.Base;
using WebStore.Domain.Identity;

namespace WebStore.Domain.Entities.Orders;

public class OrderItem : Entity
{
    [Required]
    public Product Product { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    public int Quantity { get; set; }
    
    public Order Order { get; set; }

    [NotMapped]//Exclude this property from database 
    public decimal TotalItemPrice => Price * Quantity;
}