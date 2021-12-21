using Microsoft.AspNetCore.Mvc;

namespace WebStore.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Shop/Index.cshtml");
        }

        public IActionResult ProductDetails()
        {
            return View("~/Views/Shop/ProductDetails.cshtml");
        }
    }
}
