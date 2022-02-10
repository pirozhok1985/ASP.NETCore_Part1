using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.Services.Services.Cookies;

public class CartStoreCookies : ICartStore
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly string _cartName;
    public Cart Cart
    {
        get
        {
            var cookies = _httpContextAccessor.HttpContext!.Response.Cookies;
            var cartCookies = _httpContextAccessor.HttpContext!.Request.Cookies[_cartName];
            if (cartCookies is null)
            {
                var cart = new Cart();
                cookies.Append(_cartName, JsonConvert.SerializeObject(cart));
                return cart;
            }
            ReplaceCart(cookies,cartCookies);
            return JsonConvert.DeserializeObject<Cart>(cartCookies);
        }
        set => ReplaceCart(_httpContextAccessor.HttpContext!.Response.Cookies, JsonConvert.SerializeObject(value));
    }
    private void ReplaceCart(IResponseCookies cookies, string cart)
    {
        cookies.Delete(_cartName);
        cookies.Append(_cartName,cart);
    }

    public CartStoreCookies(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        var user = httpContextAccessor.HttpContext!.User;
        var userName = user.Identity!.IsAuthenticated ? $"-{user.Identity.Name}" : null;
        _cartName = $"WebStore.GB.Cart{userName}";
    }
}