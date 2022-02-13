using Microsoft.AspNetCore.Mvc;
using WebStore.Domain;
using WebStore.Domain.DTO;
using WebStore.Interfaces.Services;
using WebStore.Services;

namespace WebStore.WebApi.Controllers;

[ApiController]
[Route(WebAddresses.Products)]
public class ProductsApiController : ControllerBase
{
    private readonly IProductData _productData;
    public ProductsApiController(IProductData productData) => _productData = productData;

    [HttpPost]
    public IActionResult GetProducts(ProductFilter? filter = null)
    {
        var result = _productData.GetProducts(filter);
        return Ok(result!.ToDto());
    }
    [HttpGet("{id}")]
    public IActionResult GetProductById(int id)
    {
        var result = _productData.GetProductById(id);
        return Ok(result?.ToDto());
    }
    
    [HttpGet("Brands")]
    public IActionResult GetBrands()
    {
        var result = _productData.GetBrands();
        return Ok(result?.ToDto());
    }
    
    [HttpGet("Brands({skip}-{take})")]
    public IActionResult GetBrands(int skip, int? take)
    {
        var result = _productData.GetBrands(skip,take);
        return Ok(result?.ToDto());
    }

    [HttpGet("brands/count")]
    public IActionResult GetBrandsCount()
    {
        var count = _productData.GetBrands()!.Count();
        return Ok(count);
    }

    [HttpGet("Brands/{id}")]
    public IActionResult GetBrandById(int id)
    {
        var result = _productData.GetBrandById(id);
        return Ok(result?.ToDto());
    }
    
    [HttpGet("Sections")]
    public IActionResult GetSections()
    {
        var result = _productData.GetSections();
        return Ok(result?.ToDto());
    }
    
    [HttpGet("Sections({skip}-{take})")]
    public IActionResult GetSections(int skip, int? take)
    {
        var result = _productData.GetSections(skip,take);
        return Ok(result?.ToDto());
    }
    
    [HttpGet("sections/count")]
    public IActionResult GetSectionsCount()
    {
        var count = _productData.GetSections()!.Count();
        return Ok(count);
    }
    
    [HttpGet("Sections/{id}")]
    public IActionResult GetSectionById(int id)
    {
        var result = _productData.GetSectionById(id);
        return Ok(result?.ToDto());
    }

    [HttpPost("new")]    
    public IActionResult Add(ProductDto product)
    {
        _productData.Add(product.FromDto()!);
        return CreatedAtAction(nameof(GetProductById), new {product.Id}, product);
    }

    [HttpPut]
    public IActionResult Update(ProductDto product)
    {
        _productData.Edit(product.FromDto()!);
        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var result = _productData.Delete(id);
        return result ? Ok(true) : NotFound(false);
    }
}