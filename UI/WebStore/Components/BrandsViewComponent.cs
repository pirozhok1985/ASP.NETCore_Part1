using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Components;

public class BrandsViewComponent : ViewComponent
{
    private readonly IEnumerable<Brand> _Brands;
    public BrandsViewComponent(IProductData brands)
    {
        _Brands = brands.GetBrands();
    }
    public IViewComponentResult Invoke() => View(GetBrands());

    private IEnumerable<BrandViewModel> GetBrands() => _Brands.OrderBy(b => b.Order).Select(b => new BrandViewModel
    {
        Id = b.Id,
        Name = b.Name,
    });
}

