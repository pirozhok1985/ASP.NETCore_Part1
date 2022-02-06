using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces.Services;
using WebStore.Services.Services;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index([FromServices]IProductData productData)
        {
            var products = productData.GetProducts().Take(3).OrderBy(p => p?.Order).ToView();
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
