using Microsoft.AspNetCore.Mvc;
using WebStore.Domain;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    public class ShopController : Controller
    {
        private readonly IProductData _ProductData;

        public ShopController(IProductData productData)
        {
            _ProductData = productData;
        }
        public IActionResult Index(int? brandId, int? sectionId)
        {
            var filter = new ProductFilter
            {
                BrandId = brandId,
                SectionId = sectionId
            };
            var products = _ProductData.GetProducts(filter);
            var productsCatalog = new CatalogViewModel
            {
                BrandId = brandId,
                SectionId = sectionId,
                Products = products.OrderBy(p => p.Order).
                    Select(p => new ProductViewModel
                    {
                        Id = p.Id,
                        ImageUrl = p.ImageUrl,
                        Price = p.Price,
                        Name = p.Name,
                    }),
            };
            return View(productsCatalog);
        }

        public IActionResult ProductDetails()
        {
            return View("~/Views/Shop/ProductDetails.cshtml");
        }

        public IActionResult Checkout()
        {
            return View("~/Views/Shop/Checkout.cshtml");
        }
        public IActionResult Cart()
        {
            return View("~/Views/Shop/Cart.cshtml");
        }
        public IActionResult Login()
        {
            return View("~/Views/Shop/Login.cshtml");
        }
    }
}
