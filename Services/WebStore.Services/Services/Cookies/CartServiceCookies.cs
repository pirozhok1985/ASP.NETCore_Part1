using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using WebStore.DAL.Migrations;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Infrastructure.Mappers;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Services.Cookies;

public class CartServiceCookies : ICartService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IProductData _productData;
    private readonly string CartName;

    private Cart Cart
    {
        get
        {
            var cookies = _httpContextAccessor.HttpContext!.Response.Cookies;
            var cartCookies = _httpContextAccessor.HttpContext!.Request.Cookies[CartName];
            if (cartCookies is null)
            {
                var cart = new Cart();
                cookies.Append(CartName, JsonConvert.SerializeObject(cart));
                return cart;
            }
            ReplaceCart(cookies,cartCookies);
            return JsonConvert.DeserializeObject<Cart>(cartCookies);
        }
        set => ReplaceCart(_httpContextAccessor.HttpContext!.Response.Cookies, JsonConvert.SerializeObject(value));
    }

    private void ReplaceCart(IResponseCookies cookies, string cart)
    {
        cookies.Delete(CartName);
        cookies.Append(CartName,cart);
    }

    public CartServiceCookies(IHttpContextAccessor httpContextAccessor, IProductData productData)
    {
        _httpContextAccessor = httpContextAccessor;
        _productData = productData;
        var user = httpContextAccessor.HttpContext!.User;
        var userName = user.Identity!.IsAuthenticated ? $"-{user.Identity.Name}" : null;
        CartName = $"WebStore.GB.Cart{userName}";
    }
    public void Add(int id)
    {
        var cart = Cart;
        var item = cart.CartItems.FirstOrDefault(i => i.ProductId == id);
        if (item is null)
        {
            cart.CartItems.Add(new CartItem() {ProductId = id, Quantity = 1});
        }
        else
            item.Quantity++;
                
        Cart = cart;
    }

    public void Decrement(int id)
    {
        var cart = Cart;
        var item = cart.CartItems.FirstOrDefault(i => i.ProductId == id);
        if(item is null)
            return;
        if (item.Quantity > 0)
            item.Quantity--;
        if (item.Quantity == 0)
            cart.CartItems.Remove(item);
        Cart = cart;
    }

    public void Remove(int id)
    {
        var cart = Cart;
        var item = cart.CartItems.FirstOrDefault(i => i.ProductId == id);
        if(item is null)
            return;
        cart.CartItems.Remove(item);
        Cart = cart;
    }

    public void Clear()
    {
        var cart = Cart;
        cart.CartItems.Clear();
        Cart = cart;
    }

    public CartViewModel GetCartViewModel()
    {
        var cart = Cart;
        var products = _productData.GetProducts(
            new ProductFilter{IDs = Cart.CartItems.Select(i => i.ProductId).ToArray()});
        var productViews = products.ToView().ToDictionary(p => p.Id);
        return new ()
        {
            Items = cart.CartItems.Where(i => productViews.ContainsKey(i.ProductId)).Select(i => (productViews[i.ProductId], i.Quantity))!
        };
    }
}