using Microsoft.AspNetCore.Mvc;

namespace WebStore.Components;

public class ShippingViewComponent : ViewComponent
{
    public IViewComponentResult Invoke() => View();
}