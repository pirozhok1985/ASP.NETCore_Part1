using WebStore.Domain;
using WebStore.Domain.Entities;

namespace WebStore.Services.Interfaces;

public interface IProductData
{
    public IEnumerable<Brand> GetBrands();
    public IEnumerable<Section> GetSections();
    public IEnumerable<Product?> GetProducts(ProductFilter? filter = null);
    public Product? GetProductById(int id);
    public void Edit(Product product);
    public void Delete(int id);
}

