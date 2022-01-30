using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces.TestAPI;

namespace WebStore.Controllers;

public class WebApiController : Controller
{
    private readonly IValueService _valueService;

    public WebApiController(IValueService valueService)
    {
        _valueService = valueService;
    }
    // GET
    public IActionResult Index()
    {
        var values = _valueService.GetValues();
        return View(values);
    }
}