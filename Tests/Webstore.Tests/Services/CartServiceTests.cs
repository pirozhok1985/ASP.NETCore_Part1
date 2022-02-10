using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using Assert = Xunit.Assert;

namespace Webstore.Tests.Services;

[TestClass]
public class CartServiceTests
{
    private Cart? _cart;
    private Mock<IProductData>? _productDataMock;

    [TestInitialize]
    private void Initialize()
    {
        _cart = new Cart
        {
            CartItems = new[]
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
}