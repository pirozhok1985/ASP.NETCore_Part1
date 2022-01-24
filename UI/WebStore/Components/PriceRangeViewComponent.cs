using Microsoft.AspNetCore.Mvc;

namespace WebStore.Components;

public class PriceRangeViewComponent : ViewComponent
{
    public IViewComponentResult Invoke() => View();
}

