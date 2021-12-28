using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities;
using WebStore.Services.Interfaces;

namespace WebStore.Components;

public class BrandsViewComponent : ViewComponent
{
    private readonly IEnumerable<Brand> _Brands;
    public BrandsViewComponent(IProductData brands)
    {
        _Brands = brands.GetBrands();
    }
    public IViewComponentResult Invoke() => View();
}

