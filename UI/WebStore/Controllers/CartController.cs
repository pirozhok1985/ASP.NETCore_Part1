using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers;

public class CartController : Controller
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService) => _cartService = cartService;
    public IActionResult Index() => View(new CartOrderViewModel{CartViewModel = _cartService.GetCartViewModel()});

    public IActionResult Add(int id)
    {
        _cartService.Add(id);
        return RedirectToAction("Index", "Cart");
    }
    public IActionResult Remove(int id)
    {
        _cartService.Remove(id);
        return RedirectToAction("Index", "Cart");
    }
    public IActionResult Decrement(int id)
    {
        _cartService.Decrement(id);
        return RedirectToAction("Index", "Cart");
    }

    [Authorize,HttpPost,ValidateAntiForgeryToken]
    public async Task<IActionResult> CheckOut(OrderViewModel orderViewModel, [FromServices] IOrderService orderService)
    {
        if (!ModelState.IsValid)
            return View(nameof(Index), new CartOrderViewModel
            {
                CartViewModel = _cartService.GetCartViewModel(),
                OrderViewModel = orderViewModel
            });
        var order = await orderService.CreateNewOrder
        (
            User.Identity.Name,
            _cartService.GetCartViewModel(),
            orderViewModel
        );
        _cartService.Clear();
        return RedirectToAction(nameof(OrderConfirmed), new {order.Id});
    }

    public IActionResult OrderConfirmed(int orderId)
    {
        ViewBag.OrderId = orderId;
        return View();
    }
}