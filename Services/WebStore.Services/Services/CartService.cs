using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Services.Services;

public class CartService : ICartService
{
    private readonly ICartStore _cartStore;
    private readonly IProductData _productData;
    
    public CartService(ICartStore cartStore, IProductData productData)
    {
        _cartStore = cartStore;
        _productData = productData;
    }

    public void Add(int id)
    {
        var cart = _cartStore.Cart;
        var item = cart.CartItems.FirstOrDefault(i => i.ProductId == id);
        if (item is null)
        {
            cart.CartItems.Add(new CartItem() {ProductId = id, Quantity = 1});
        }
        else
            item.Quantity++;
                
        _cartStore.Cart = cart;
    }

    public void Decrement(int id)
    {
        var cart = _cartStore.Cart;
        var item = cart.CartItems.FirstOrDefault(i => i.ProductId == id);
        if(item is null)
            return;
        if (item.Quantity > 0)
            item.Quantity--;
        if (item.Quantity == 0)
            cart.CartItems.Remove(item);
        _cartStore.Cart = cart;
    }

    public void Remove(int id)
    {
        var cart = _cartStore.Cart;
        var item = cart.CartItems.FirstOrDefault(i => i.ProductId == id);
        if(item is null)
            return;
        cart.CartItems.Remove(item);
        _cartStore.Cart = cart;
    }

    public void Clear()
    {
        var cart = _cartStore.Cart;
        cart.CartItems.Clear();
        _cartStore.Cart = cart;
    }

    public CartViewModel GetCartViewModel()
    {
        var cart = _cartStore.Cart;
        var products = _productData.GetProducts(
            new ProductFilter{IDs = _cartStore.Cart.CartItems.Select(i => i.ProductId).ToArray()});
        var productViews = products.Products.ToView().ToDictionary(p => p.Id);
        return new ()
        {
            Items = cart.CartItems.Where(i => productViews.ContainsKey(i.ProductId)).Select(i => (productViews[i.ProductId], i.Quantity))!
        };
    }
}