using Microsoft.AspNetCore.Mvc;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.WebApi.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsApiController : Controller
{
    private readonly IProductData _productData;
    public ProductsApiController(IProductData productData) => _productData = productData;

    [HttpPost]
    public IActionResult GetProducts(ProductFilter? filter = null)
    {
        var result = _productData.GetProducts(filter);
        return Ok(result);
    }
    [HttpGet("{id}")]
    public IActionResult GetProductById(int id)
    {
        var result = _productData.GetProductById(id);
        return Ok(result);
    }
    
    [HttpGet("Brands")]
    public IActionResult GetBrands()
    {
        var result = _productData.GetBrands();
        return Ok(result);
    }
    
    [HttpGet("Sections")]
    public IActionResult GetSections()
    {
        var result = _productData.GetSections();
        return Ok(result);
    }

    [HttpPost("new")]    
    public IActionResult Add(Product product)
    {
        _productData.Add(product);
        return CreatedAtAction(nameof(GetProductById), new {product.Id}, product);
    }

    [HttpPut]
    public IActionResult Update(Product product)
    {
        _productData.Edit(product);
        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var result = _productData.Delete(id);
        return result ? Ok(true) : NotFound(false);
    }
}