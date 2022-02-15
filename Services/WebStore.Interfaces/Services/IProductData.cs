using WebStore.Domain;
using WebStore.Domain.Entities;

namespace WebStore.Interfaces.Services;

public interface IProductData
{
    IEnumerable<Brand>? GetBrands(int skip = 0, int? take = null);
    int GetBrandsCount();
    IEnumerable<Section>? GetSections(int skip = 0, int? take = null);
    int GetSectionsCount();
    ProductsPage GetProducts(ProductFilter? filter = null);
    Product? GetProductById(int id);
    Section? GetSectionById(int? id);
    Brand? GetBrandById(int id);
    void Edit(Product product);
    bool Delete(int id);
    void Add(Product product);
}

