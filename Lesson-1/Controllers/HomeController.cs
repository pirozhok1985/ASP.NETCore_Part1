using Microsoft.AspNetCore.Mvc;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _config;
        public HomeController(IConfiguration config)
        {
            _config = config;
        }
        public IActionResult Index()
        {
            //return Content(_config.GetValue<string>("CustomGreetings"));
            return View();
        }

        public IActionResult CustomAction(string id)
        {
            return Content($"Hello! World! {id}");
        }

        public IActionResult Throw(string message) => throw new ApplicationException(message);
    }
}
