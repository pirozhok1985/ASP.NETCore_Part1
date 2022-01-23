using Microsoft.AspNetCore.Mvc;

namespace WebStore.Controllers;

public class UserProfile : Controller
{
    public IActionResult Index() => View();
}