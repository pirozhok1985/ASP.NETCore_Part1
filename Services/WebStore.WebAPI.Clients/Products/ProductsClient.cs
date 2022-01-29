using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.WebAPI.Clients.Products;

public class ProductsClient : IProductData
{
    public IEnumerable<Brand> GetBrands()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Section> GetSections()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Product?> GetProducts(ProductFilter? filter = null)
    {
        throw new NotImplementedException();
    }

    public Product? GetProductById(int id)
    {
        throw new NotImplementedException();
    }

    public void Edit(Product product)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public void Add(Product product)
    {
        throw new NotImplementedException();
    }
}