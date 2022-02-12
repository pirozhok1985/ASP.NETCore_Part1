using Microsoft.AspNetCore.Mvc;

namespace WebStore.Components;

public class BreadCrumbsViewComponent : ViewComponent
{
    IViewComponentResult Invoke() => View();
}