using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using WebStore.Domain.Entities;

namespace WebStore.ViewModels;

public class ProductViewModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Имя обязательно к заполнению")]
    [StringLength(maximumLength: 255, ErrorMessage = "Значение должно быть в диапазоне от 2-х до 255 символов", MinimumLength = 2)]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Цена обязательна к заполнению")]
    [Range(10, 5000, ErrorMessage = "Укажите цену из допустимого диапазона" )]
    public decimal Price { get; set; }
    public string ImageUrl { get; set; }
    public string Section { get; set; }
    public string? Brand { get; set; }
    public int SectionId { get; set; }
    public int? BrandId { get; set; }
}

