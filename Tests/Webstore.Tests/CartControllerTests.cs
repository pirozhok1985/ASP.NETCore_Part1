using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebStore.Controllers;
using WebStore.Domain.Entities.Orders;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using Assert = Xunit.Assert;

namespace Webstore.Tests;

[TestClass]
public class CartControllerTests
{
    [TestMethod]
    public async Task CheckOutWithInvalidModelState()
    {
        var expectedDescription = "Test Description";
        var cartServiceMock = new Mock<ICartService>();
        var orderServiceMock = new Mock<IOrderService>();
        var controller = new CartController(cartServiceMock.Object);
        controller.ModelState.AddModelError("error", "Test model error");
        var orderViewModel = new OrderViewModel
        {
            Description = expectedDescription
        };
        
        var result = await controller.CheckOut(orderViewModel, orderServiceMock.Object);
        
        var resultView = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<CartOrderViewModel>(resultView.Model);
        Assert.Equal(expectedDescription,model.OrderViewModel.Description);
        cartServiceMock.Verify(c => c.GetCartViewModel());
        cartServiceMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task CheckOutWithValidModelState()
    {
        var expectedUserName = "Test User";
        var expectedOrderId = 1;
        var expectedDescription = "Test Description";
        var expecedPhone = "Test Phone number";
        var expectedAddress = "Test Address";
        var cartServiceMock = new Mock<ICartService>();
        cartServiceMock.Setup(c => c.GetCartViewModel()).Returns(new CartViewModel
        {
            Items = new []{(new ProductViewModel { Name = "Test Product"},1)}
        });
        var orderServiceMock = new Mock<IOrderService>();
        orderServiceMock.Setup(o => o.CreateNewOrderAsync(
            It.IsAny<string>(),
            It.IsAny<CartViewModel>(),
            It.IsAny<OrderViewModel>(),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(new Order
        {
            Id = expectedOrderId,
            Address = expectedAddress,
            Date = DateTimeOffset.Now,
            Description = expectedDescription,
            Phone = expecedPhone,
            Items = Array.Empty<OrderItem>()
        });
        var controller = new CartController(cartServiceMock.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal
                    (
                        new ClaimsIdentity(new [] {new Claim(ClaimTypes.Name,expectedUserName)})
                    )
                }
            }
        };
        
        var orderViewModel = new OrderViewModel
        {
            Address = expectedAddress,
            Description = expectedDescription,
            Phone = expecedPhone
        };
 
        
        var result = await controller.CheckOut(orderViewModel, orderServiceMock.Object);
        var resultView = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(nameof(controller.OrderConfirmed),resultView.ActionName);
        Assert.Equal(expectedOrderId, resultView.RouteValues["id"]);
    }
}