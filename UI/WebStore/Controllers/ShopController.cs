using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Services;

namespace WebStore.Controllers
{
    public class ShopController : Controller
    {
        private readonly IProductData _ProductData;
        private readonly IConfiguration _configuration;

        public ShopController(IProductData productData, IConfiguration configuration)
        {
            _ProductData = productData;
            _configuration = configuration;
        }
        public IActionResult Index(int? brandId, int? sectionId, int page = 1, int? pageSize = null)
        {
            var filter = new ProductFilter
            {
                BrandId = brandId,
                SectionId = sectionId,
                Page = page,
                PageSize = pageSize ?? 
                           (int.TryParse(_configuration["CatalogPageSize"], out var pSize)? pSize : null)
            };
            var (products,totalCount) = _ProductData.GetProducts(filter);
            var productsCatalog = new CatalogViewModel
            {
                BrandId = brandId,
                SectionId = sectionId,
                Products = products.OrderBy(p => p.Order).ToView(),
                PageViewModel = new PageViewModel
                {
                    Page = page,
                    PageSize = filter.PageSize ?? 0,
                    TotalCount = totalCount
                }
            };
            return View(productsCatalog);
        }

        public IActionResult Checkout()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            var product = _ProductData.GetProductById(id);
            if (product is null)
                return NotFound();
            return View(product.ToView());
        }
    }
}
