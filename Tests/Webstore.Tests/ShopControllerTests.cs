using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebStore.Controllers;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using Assert = Xunit.Assert;

namespace Webstore.Tests;

[TestClass]
public class ShopControllerTests
{
    [TestMethod]
    public void DetailsReturnsProductViewNodel()
    {
        const int id = 13;
        const string expectedName = "Белое платье";
        const string expectedImageUrl = "product7.jpg";
        const decimal expectedPrice = 1386;
        const int expectedSectionId = 2;
        const int expectedBrandId = 1;
        const string expectedSectionName = "Nike";
        const int expectedSectionParentId = 1;
        const string expectedBrandName = "Acne";
        
        var productDataMock = new Mock<IProductData>();
        productDataMock.Setup(p => p.GetProductById(It.Is<int>(itemId => itemId > 0)))
            .Returns<int>(pId => new Product
            {
                Id = pId,
                Name = expectedName,
                ImageUrl = expectedImageUrl,
                Price = expectedPrice,
                BrandId = expectedBrandId,
                SectionId = expectedSectionId,
                Brand = new Brand
                {
                    Id = expectedBrandId,
                    Name = expectedBrandName,
                },
                Section = new Section
                {
                    Id = expectedSectionId,
                    Name = expectedSectionName,
                    ParentId = expectedSectionParentId
                }
            });
        
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(c => c[It.IsAny<string>()])
            .Returns("3");
        
        var controller = new ShopController(productDataMock.Object, configurationMock.Object);
        
        
        var result = controller.Details(id);
        
        
        var viewResult = Assert.IsType<ViewResult>(result);
        var productViewModel = Assert.IsType<ProductViewModel>(viewResult.Model);
        Assert.Equal(id,productViewModel.Id);
        Assert.Equal(expectedName,productViewModel.Name);
        Assert.Equal(expectedPrice,productViewModel.Price);
        Assert.Equal(expectedImageUrl,productViewModel.ImageUrl);
        Assert.Equal(expectedBrandName,productViewModel.Brand);
        Assert.Equal(expectedBrandId,productViewModel.BrandId);
        Assert.Equal(expectedSectionName,productViewModel.Section);
        Assert.Equal(expectedSectionId,productViewModel.SectionId);
        
        productDataMock.Verify(p => p.GetProductById(It.Is<int>(itemId => itemId > 0)));
    }
}