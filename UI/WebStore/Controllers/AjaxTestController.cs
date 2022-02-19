using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.ViewModels;

namespace WebStore.Controllers;

public class AjaxTestController : Controller
{
    private readonly ILogger<AjaxTestController> _logger;

    public AjaxTestController(ILogger<AjaxTestController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index() => View();
    
    public async Task<IActionResult> GetJson(int? id, string message, int delay = 2000)
    {
        _logger.LogInformation("Start processing GetJson request with id {0} - {1}, delay is {2}", id, message, delay);
        if (delay != 0)
            await Task.Delay(delay);
        _logger.LogInformation("Finish processing GetJson request with id {0} - {1}, delay is {2}", id, message, delay);
        return new JsonResult(new
            {
                Message = $"Response({id ?? 0}) : {message ?? "--null--"}", ServerTime = DateTime.Now
            });
    }

    public async Task<IActionResult> GetHtml(int? id, string message, int delay = 2000)
    {
        _logger.LogInformation("Start processing GetHtml request with id {0} - {1}, delay is {2}", id, message, delay);
        if (delay != 0)
            await Task.Delay(delay);
        _logger.LogInformation("Finish processing GetHtml request with id {0} - {1}, delay is {2}", id, message, delay);
        return PartialView("Partial/_DataView", new AjaxTestViewModel
        {
            Id = id ?? -1,
            Message = message ?? "--null--",
            ServerTime = DateTime.Now
        });
    }

    public IActionResult Chat() => View();
}