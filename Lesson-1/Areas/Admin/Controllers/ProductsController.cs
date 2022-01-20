using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Identity;
using WebStore.Infrastructure.Mappers;
using WebStore.Services.Interfaces;

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
        var productView = _productData.GetProducts().ToView();
        return  View(productView);
    }

    
}