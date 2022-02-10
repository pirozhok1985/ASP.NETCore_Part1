using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Services;
using Assert = Xunit.Assert;

namespace Webstore.Tests.Services;

[TestClass]
public class CartServiceTests
{
    private Cart? _cart;
    private Mock<IProductData>? _productDataMock;
    private Mock<ICartStore> _cartStoreMock;
    private ICartService _cartService;

    [TestInitialize]
    public void Initialize()
    {
        _cart = new Cart
        {
            CartItems = new List<CartItem>
            {
                new CartItem{ProductId = 1, Quantity = 1},
                new CartItem{ProductId = 2, Quantity = 3}
            }
        };
        _productDataMock = new Mock<IProductData>();
        _productDataMock
            .Setup(p => p.GetProducts(It.IsAny<ProductFilter>()))
            .Returns(new []
            {
                new Product
                {
                    Id = 1,
                    Brand = new Brand{Id = 1,Name = "Nike",Order = 1},
                    Name = "Snickers",
                    Order = 1,
                    Price = 1500,
                    Section = new Section{Id = 1,Name = "Sport",Order = 1},
                    BrandId = 1,
                    ImageUrl = "product6.img",
                    SectionId = 1
                },
                new Product
                {
                    Id = 2,
                    Brand = new Brand{Id = 2,Name = "Adidas",Order = 2},
                    Name = "Snickers",
                    Order = 2,
                    Price = 2500,
                    Section = new Section{Id = 1,Name = "Sport",Order = 2},
                    BrandId = 2,
                    ImageUrl = "product5.img",
                    SectionId = 1
                },
                new Product
                {
                    Id = 3,
                    Brand = new Brand{Id = 3,Name = "Puma",Order = 3},
                    Name = "Snickers",
                    Order = 3,
                    Price = 2000,
                    Section = new Section{Id = 1,Name = "Sport",Order = 3},
                    BrandId = 3,
                    ImageUrl = "product4.img",
                    SectionId = 1
                }
            });
        _cartStoreMock = new Mock<ICartStore>();
        _cartStoreMock
            .Setup(c => c.Cart)
            .Returns(_cart);
        _cartService = new CartService(_cartStoreMock.Object, _productDataMock.Object);
    }

    [TestMethod]
    public void CartItemsCountReturnsCorrectValue()
    {
        var cart = _cart;
        var expectedQuantity = cart!.CartItems.Sum(i => i.Quantity);
        var actualQuantity = cart.ItemsSum;
        Assert.Equal(expectedQuantity,actualQuantity);
    }

    [TestMethod]
    public void CartViewModelItemCountReturnsCorrectValue()
    {
        var cartViewModel = new CartViewModel
        {
            Items = new (ProductViewModel product, int Quantity)[]
            {
               (new ProductViewModel
               {
                   Id = 1,
                   Brand = "Nike",
                   Name = "Snickers",
                   Price = 1500,
                   Section = "Sport",
                   BrandId = 1,
                   ImageUrl = "product5.img",
                   SectionId = 1
               },1),
               (new ProductViewModel
               {
                   Id = 2,
                   Brand = "Adidas",
                   Name = "Snickers",
                   Price = 2500,
                   Section = "Sport",
                   BrandId = 2,
                   ImageUrl = "product4.img",
                   SectionId = 1   
               },3)
            },
        };
        var expectedItemCount = cartViewModel.Items.Sum(i => i.Quantity);
        var actualItemsCount = cartViewModel.ItemCount;
        Assert.Equal(expectedItemCount,actualItemsCount);
    }
    
    [TestMethod]
    public void CartViewModelTotalPriceReturnsCorrectValue()
    {
        var cartViewModel = new CartViewModel
        {
            Items = new (ProductViewModel product, int Quantity)[]
            {
                (new ProductViewModel
                {
                    Id = 1,
                    Brand = "Nike",
                    Name = "Snickers",
                    Price = 1500,
                    Section = "Sport",
                    BrandId = 1,
                    ImageUrl = "product5.img",
                    SectionId = 1
                },1),
                (new ProductViewModel
                {
                    Id = 2,
                    Brand = "Adidas",
                    Name = "Snickers",
                    Price = 2500,
                    Section = "Sport",
                    BrandId = 2,
                    ImageUrl = "product4.img",
                    SectionId = 1   
                },3)
            },
        };
        var expectedItemCount = cartViewModel.Items.Sum(i => i.Quantity * i.product.Price);
        var actualItemsCount = cartViewModel.TotalPrice;
        Assert.Equal(expectedItemCount,actualItemsCount);
    }

    [TestMethod]
    public void CartCheckAddMethod()
    {
        _cart.CartItems.Clear();
        const int expectedId = 5;
        const int expectedItems = 1;
        
        _cartService.Add(expectedId);
        
        var actualItems = _cart.ItemsSum;
        Assert.Equal(expectedItems, actualItems);
        Assert.Single(_cart.CartItems);
        Assert.Equal(expectedId,_cart.CartItems.Single().ProductId);
    }
    
    [TestMethod]
    public void CartCheckRemoveMethod()
    {
        var itemForRemove = 2;
        const int expectedId = 1;
        var actualItems = _cart.ItemsSum;
        
        _cartService.Remove(itemForRemove);
        Assert.Single(_cart.CartItems);
        Assert.Equal(expectedId,_cart.CartItems.Single().ProductId);
    }
    
    [TestMethod]
    public void CartCheckClearMethod()
    {
        _cart.CartItems.Clear();
        Assert.Empty(_cart.CartItems);
    }

    [TestMethod]
    public void CartCheckDecrementMethod()
    {
        var itemId = 2;
        const int expectedItemsCount = 3;
        const int expectedProductsCount = 2;
        const int expectedQuantity = 2;
        
        _cartService.Decrement(itemId);
        var actualItemsCount = _cart.ItemsSum;
        var items = _cart.CartItems.ToArray();
        
        Assert.Equal(expectedItemsCount,actualItemsCount);
        Assert.Equal(expectedProductsCount, _cart.CartItems.Count);
        Assert.Equal(itemId,items[1].ProductId);
        Assert.Equal(expectedQuantity, items[1].Quantity);
    }

    [TestMethod]
    public void CartCheckRemoveAfterDecrementMethod()
    {
        var itemId = 1;
        const int expectedItemsCount = 3;
        
        _cartService.Decrement(itemId);
        Assert.Single(_cart.CartItems);
        Assert.Equal(expectedItemsCount, _cart.ItemsSum);
    }

    [TestMethod]
    public void CartCheckGetCartViewModelMethod()
    {
        const int expectedItemsCount = 4;
        const decimal expectedFirstItemPrice = 1500;

        var cartViewModel = _cartService.GetCartViewModel();
        Assert.Equal(expectedItemsCount,cartViewModel.ItemCount);
        Assert.Equal(expectedFirstItemPrice,cartViewModel.Items.First().product.Price);
    }
}