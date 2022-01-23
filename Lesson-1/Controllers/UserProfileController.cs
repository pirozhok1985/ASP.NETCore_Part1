using Microsoft.AspNetCore.Mvc;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Controllers;

public class UserProfile : Controller
{
    public IActionResult Index() => View();

    public async Task<IActionResult> Orders([FromServices] IOrderService orderService)
    {
        var orders = await orderService.GetUserOrdersAsync(User.Identity.Name);
        return View(orders.Select(o => new UserOrderViewModel
        {
            Id = o.Id,
            Address = o.Address,
            Description = o.Description,
            Phone = o.Phone,
            TotalPrice = o.TotalPrice,
            Date = o.Date,
        }));
    }
}