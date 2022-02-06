using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebStore.Controllers;
using WebStore.Interfaces.TestAPI;
using Assert = Xunit.Assert;

namespace Webstore.Tests;

[TestClass]
public class WebApiControllerTests
{
    [TestMethod]
    public void IndexReturnsViewWithData()
    {
        var data = Enumerable.Range(1, 10).Select(v => $"Value-{v}").ToArray();
        var valueServiceMock = new Mock<IValueService>();
        valueServiceMock.Setup(s => s.GetValues()).Returns(data);
        var controller = new WebApiController(valueServiceMock.Object);

        var result = controller.Index();
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<string>>(viewResult.Model);
        var i = 0;
        foreach (var actualResult in model)
        {
            var expectedResult = data[i++];
            Assert.Equal(expectedResult,actualResult);
        }
        valueServiceMock.Verify(s => s.GetValues());
        valueServiceMock.VerifyNoOtherCalls();
    }
}