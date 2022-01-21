using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Services.Interfaces;

namespace WebStore.Services.Database;

public class ProductDataDB : IProductData
{
    private readonly WebStoreDB _db;
    private readonly ILogger<ProductDataDB> _logger;

    public ProductDataDB(WebStoreDB db, ILogger<ProductDataDB> logger)
    {
        _db = db;
        _logger = logger;
    }

    public IEnumerable<Brand> GetBrands() => _db.Brands;

    public IEnumerable<Section> GetSections() => _db.Sections;

    public IEnumerable<Product?> GetProducts(ProductFilter? filter = null)
    {
        IQueryable<Product> query = _db.Products
            .Include(p => p.Brand)
            .Include(p => p.Section);
        if (filter?.IDs?.Length > 0)
            query = query.Where(p => filter.IDs.Contains(p.Id));
        
        else
        {
            if (filter?.SectionId is { } section_id)
                query = query.Where(p => p.SectionId == section_id);

            if (filter?.BrandId is { } brand_id)
                query = query.Where(p => p.BrandId == brand_id);
        }

        return query;
    }

    public Product? GetProductById(int id)
    {
        return _db.Products
            .Include(p => p.Brand)
            .Include(p => p.Section)
            .FirstOrDefault(p => p.Id == id);
    }

    public void Edit(Product product)
    {
        if (product is null)
            return;
        _db.Products.Update(product);
        _db.SaveChanges();
    }

    public void Delete(Product product)
    {
        if (product is null)
            return;
        _db.Products.Remove(product);
        _db.SaveChanges();
    }
}