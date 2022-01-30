using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;

namespace WebStore.Services.Services;

public static class ProductMapper
{
    public static ProductViewModel? ToView(this Product? product) => product is null
    ? null
    :new ProductViewModel
    {
            Id = product.Id,
            ImageUrl = product.ImageUrl,
            Price = product.Price,
            Name = product.Name,
            Brand = product.Brand.Name,
            Section = product.Section.Name,
            BrandId = product.BrandId,
            SectionId = product.SectionId
        };

    public static Product? FromView(this ProductViewModel? product) => product is null
    ? null
    : new Product
        {
            Id = product.Id,
            ImageUrl = product.ImageUrl,
            Price = product.Price,
            Name = product.Name,
            Brand = new Brand{Name = product.Brand},
            Section = new Section{Name = product.Section},
            SectionId = product.SectionId
        };
    

    public static IEnumerable<ProductViewModel?> ToView(this IEnumerable<Product?>? products) =>
        products.Select(p => p.ToView());
    public static IEnumerable<Product?> FromView(this IEnumerable<ProductViewModel?> products) =>
        products.Select(p => p.FromView());
}