using WebStore.Domain.Entities;

namespace WebStore.Domain.DTO;

public class ProductDto
{
    public int Id { get; set; }
    public int Order { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public decimal Price { get; set; }
    public SectionDto Section { get; set; }
    public BrandDto? Brand { get; set; }
}

public static class ProductDtoMapper
{
    public static ProductDto ToDto(this Product product) => product is null
        ? null
        : new ProductDto
        {
            Id = product.Id,
            Order = product.Order,
            Name = product.Name,
            ImageUrl = product.ImageUrl,
            Section = product.Section.ToDto()!,
            Brand = product.Brand.ToDto(),
            Price = product.Price,
        };
    public static Product? FromDto(this ProductDto? product) => product is null
        ? null
        : new Product
        {
            Id = product.Id,
            Order = product.Order,
            Name = product.Name,
            ImageUrl = product.ImageUrl,
            Section = product.Section.FromDto()!,
            Brand = product.Brand.FromDto(),
            BrandId = product.Brand?.Id,
            SectionId = product.Section.Id,
            Price = product.Price,
        };

    public static IEnumerable<ProductDto> ToDto(this IEnumerable<Product> products) => products.Select(ToDto);
    public static IEnumerable<Product?> FromDto(this IEnumerable<ProductDto?> products) => products.Select(FromDto);
}