using WebStore.Domain;
using WebStore.Domain.Entities;

namespace WebStore.Interfaces.Services;

public interface IProductData
{
    public IEnumerable<Brand>? GetBrands();
    public IEnumerable<Section>? GetSections();
    public IEnumerable<Product?>? GetProducts(ProductFilter? filter = null);
    public Product? GetProductById(int id);
    public void Edit(Product product);
    public bool Delete(int id);
    public void Add(Product product);
}

