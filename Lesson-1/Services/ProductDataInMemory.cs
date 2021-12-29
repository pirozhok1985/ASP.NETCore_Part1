using WebStore.Data;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Services.Interfaces;

namespace WebStore.Services;

public class ProductDataInMemory : IProductData
{
    public IEnumerable<Brand> GetBrands() => TestData.Brands;

    public IEnumerable<Product> GetProducts(ProductFilter? filter = null)
    {
        IEnumerable<Product> filteredProducts = TestData.Products;
        if (filter?.SectionId != null)
            filteredProducts = filteredProducts.Where(p => p.SectionId == filter.SectionId);
        if (filter?.BrandId != null)
            filteredProducts = filteredProducts.Where(p => p.BrandId == filter.BrandId);
        return filteredProducts;
    }

    public IEnumerable<Section> GetSections() => TestData.Sections;
}

