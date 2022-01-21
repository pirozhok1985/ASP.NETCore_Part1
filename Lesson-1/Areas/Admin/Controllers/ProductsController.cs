using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using WebStore.Domain.Identity;
using WebStore.Infrastructure.Mappers;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Areas.Admin.Controllers;

[Area("Admin"), Authorize(Roles = Role.Administrators)]
public class ProductsController : Controller
{
    private readonly IProductData _productData;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductData productData, ILogger<ProductsController> logger)
    {
        _productData = productData;
        _logger = logger;
    }

    public IActionResult Index()
    {
        var productsView = _productData.GetProducts().ToView();
        return  View(productsView);
    }

    public IActionResult Edit(int id)
    {
        var productView = _productData.GetProductById(id).ToView();
        return View(productView);
    }

    [HttpPost]
    public IActionResult Edit(ProductViewModel productView)
    {
        var product = productView.FromView();
        _productData.Edit(product);
        return RedirectToAction("Index");
    }
}