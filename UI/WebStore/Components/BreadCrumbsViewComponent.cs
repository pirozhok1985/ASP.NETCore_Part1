using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Components;

public class BreadCrumbsViewComponent : ViewComponent
{
    private readonly IProductData _productData;

    public BreadCrumbsViewComponent(IProductData productData) => _productData = productData;

    public IViewComponentResult Invoke()
    {
        var breadCrumbs = new BreadCrumbsViewModel();
        if (int.TryParse(HttpContext.Request.Query["sectionId"], out var sectionId))
        {
            breadCrumbs.Section = _productData.GetSectionById(sectionId);
            if (breadCrumbs.Section!.ParentId is { } sectionParentId && breadCrumbs.Section.Parent is null)
                breadCrumbs.Section.Parent = _productData.GetSectionById(sectionParentId);
        }

        if (int.TryParse(HttpContext.Request.Query["brandId"], out var brandId))
        {
            breadCrumbs.Brand = _productData.GetBrandById(brandId);
        }


        if (int.TryParse(HttpContext.Request.RouteValues["id"]?.ToString(), out var productId))
        {
            breadCrumbs.ProductName = _productData.GetProductById(productId)!.Name;
        }

        return View(breadCrumbs);
    }


}