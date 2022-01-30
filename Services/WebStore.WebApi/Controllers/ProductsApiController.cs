using Microsoft.AspNetCore.Mvc;
using WebStore.Domain;
using WebStore.Domain.DTO;
using WebStore.Interfaces.Services;

namespace WebStore.WebApi.Controllers;

[ApiController]
[Route("api/products")]
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
    
    [HttpGet("Sections")]
    public IActionResult GetSections()
    {
        var result = _productData.GetSections();
        return Ok(result?.ToDto());
    }

    [HttpPost("new")]    
    public IActionResult Add(ProductDto product)
    {
        _productData.Add(product!.FromDto()!);
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