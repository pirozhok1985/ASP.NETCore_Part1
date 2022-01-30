using WebStore.Domain.Entities;

namespace WebStore.Domain.DTO;

public class BrandDto
{
    public int Id { get; set; }
    public int Order { get; set; }
    public string Name { get; set; }
}

public static class BrandDtoMapper
{
    public static BrandDto? ToDto(this Brand? brand) => brand is null 
        ? null
        : new BrandDto
        {
            Id = brand.Id,
            Order = brand.Order,
            Name = brand.Name,
        };
    public static Brand? FromDto(this BrandDto? brand) => brand is null 
        ? null
        : new Brand
        {
            Id = brand.Id,
            Order = brand.Order,
            Name = brand.Name,
        };

    public static IEnumerable<BrandDto?> ToDto(this IEnumerable<Brand?> brands) => brands.Select(ToDto);
    public static IEnumerable<Brand?> FromDto(this IEnumerable<BrandDto?> brands) => brands.Select(FromDto);
}

