using Microsoft.AspNetCore.Identity;
using WebStore.Domain.Entities;

namespace WebStore.Domain.DTO;

public record ProductsPageDto(IEnumerable<ProductDto> Products, int TotalCount);

public static class ProductsPageDtoMapper
{
    public static ProductsPageDto ToDto(this ProductsPage page) => new(page.Products.ToDto(), page.TotalCount);
    public static ProductsPage FromDto(this ProductsPageDto page) => new(page.Products.FromDto()!, page.TotalCount);
}