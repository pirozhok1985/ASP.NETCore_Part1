using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebStore.Interfaces.Services;
using WebStore.Services.Services;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index([FromServices]IProductData productData)
        {
            var products = productData.GetProducts(new () { Page = 1, PageSize = 6}).Products.OrderBy(p => p?.Order).ToView();
            ViewBag.Products = products;
            return View();
        }

        public IActionResult CustomAction(string id)
        {
            return Content($"Hello! World! {id}");
        }

        public IActionResult Throw(string message) => throw new ApplicationException(message);
    }
}
