using Microsoft.AspNetCore.Mvc;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _config;
        public HomeController(IConfiguration config)
        {
            _config = config;
        }
        public IActionResult Index([FromServices]IProductData productData)
        {
            //return Content(_config.GetValue<string>("CustomGreetings"));
            var products = productData.GetProducts().Take(3).OrderBy(p => p.Order).Select(p => new ProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                ImageUrl = p.ImageUrl,
                Price = p.Price,
            });
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
