using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces.TestAPI;

namespace WebStore.Controllers;

public class WebAPIController : Controller
{
    private readonly IValueService _valueService;

    public WebAPIController(IValueService valueService)
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