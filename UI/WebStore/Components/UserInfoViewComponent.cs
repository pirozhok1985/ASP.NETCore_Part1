using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Identity;

namespace WebStore.Components;

public class UserInfoViewComponent : ViewComponent
{
    public IViewComponentResult Invoke() => User.Identity?.IsAuthenticated == true ? View("UserInfo") : View();
}