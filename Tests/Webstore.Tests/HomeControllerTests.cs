using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebStore.Controllers;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using Assert = Xunit.Assert;

namespace Webstore.Tests;

[TestClass]
public class HomeControllerTests
{
    [TestMethod]
    public void IndexReturnsView()
    {
        var productDataMock = new Mock<IProductData>();
        productDataMock.Setup(p => p.GetProducts(It.IsAny<ProductFilter>()))
            .Returns(Enumerable.Empty<Product?>());

        var actualResult = new HomeController().Index(productDataMock.Object);
        Assert.IsType<ViewResult>(actualResult);
    }

    [TestMethod]
    public void CustomActionReturnsContent()
    {
        const string id = "123";
        var controller = new HomeController();
        
        var resultValue = controller.CustomAction(id);
        
        Assert.IsType<ContentResult>(resultValue);
    }

}