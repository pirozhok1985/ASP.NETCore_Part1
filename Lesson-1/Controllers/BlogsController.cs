using Microsoft.AspNetCore.Mvc;

namespace WebStore.Controllers
{
    public class BlogsController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Title = "Blog List";
            return View("~/Views/Blogs/Index.cshtml");
        }

        public IActionResult Blog()
        {
            ViewBag.Title = "Blog Single";
            return View("~/Views/Blogs/Blog.cshtml");
        }
    }
}
