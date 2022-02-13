using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using WebStore.Domain.Identity;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Services;

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
        var productsView = _productData.GetProducts().Products.ToView();
        return View(productsView);
    }

    public IActionResult Edit(int id)
    {
        var productView = _productData.GetProductById(id).ToView();
        return View(productView);
    }

    [HttpPost]
    public IActionResult Edit(ProductViewModel productView)
    {
        if (!ModelState.IsValid)
            return View(productView);
        var product = productView.FromView();
        var brand = _productData.GetBrands().FirstOrDefault(b => b.Name.Equals(product.Brand.Name));
        var section = _productData.GetSections().FirstOrDefault(s => s.Name.Equals(product.Section.Name));
        product.Brand = brand;
        product.Section = section;
        _productData.Edit(product);
        return RedirectToAction("Index");
    }

    public IActionResult Delete(int id)
    {
        var productView = _productData.GetProductById(id).ToView();
        return View(productView);
    }
    
    [HttpPost]
    public IActionResult Delete(ProductViewModel productView)
    {
        var product = productView.FromView();
        _productData.Delete(product.Id);
        return RedirectToAction("Index");
    }
    
    public IActionResult Add(string name, string? brand, string section, string? imageUrl, decimal price)
    {
        var productView = new ProductViewModel
        {
            Name = name,
            Brand = brand,
            Section = section,
            ImageUrl = imageUrl,
            Price = price
        };
        return View(productView);
    }
    
    [HttpPost]
    public IActionResult Add(ProductViewModel productView)
    {
        if (!ModelState.IsValid)
            return View(productView);
        var product = productView.FromView();
        var brand = _productData.GetBrands().FirstOrDefault(b => b.Name.Equals(product.Brand.Name));
        var section = _productData.GetSections().FirstOrDefault(s => s.Name.Equals(product.Section.Name));
        product.Brand = brand;
        product.Section = section;
        _productData.Add(product);
        return RedirectToAction("Index");
    }
}