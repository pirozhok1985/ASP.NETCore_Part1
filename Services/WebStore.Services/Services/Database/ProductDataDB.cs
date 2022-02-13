using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebStore.DAL.Context;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.Services.Services.Database;

public class ProductDataDB : IProductData
{
    private readonly WebStoreDB _db;
    private readonly ILogger<ProductDataDB> _logger;

    public ProductDataDB(WebStoreDB db, ILogger<ProductDataDB> logger)
    {
        _db = db;
        _logger = logger;
    }

    public IEnumerable<Brand>? GetBrands(int skip, int? take)
    {
        IQueryable<Brand> query = _db.Brands!;
        if (skip > 0)
            query = query.Skip(skip);
        if (take is not null)
            query = query.Take((int)take);
        return query;
    }

    public IEnumerable<Section>? GetSections(int skip, int? take)
    {
        IQueryable<Section> query = _db.Sections!;
        if (skip > 0)
            query = query.Skip(skip);
        if (take is not null)
            query = query.Take((int)take);
        return query;
    }

    public Section? GetSectionById(int? sectionId)
    {
        var result = _db.Sections!.Find(sectionId);
        return _db.Sections!.FirstOrDefault(s => s.Id == sectionId);
    }

    public Brand? GetBrandById(int brandId)
    {
        return _db.Brands!.FirstOrDefault(b => b.Id == brandId);
    }

    public int GetBrandsCount() => _db.Brands!.Count();

    public int GetSectionsCount() => _db.Sections!.Count();

    public ProductsPage GetProducts(ProductFilter? filter = null)
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

        var itemsCount = query.Count();
        if (filter is {Page: > 0 and var page, PageSize: > 0 and var pageSize})
            query = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        return new(query.AsEnumerable(),itemsCount);
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

    public bool Delete(int id)
    {
        var product = GetProductById(id);
        _db.Products.Remove(product);
        var result = _db.SaveChanges();
        return result != 0 ? true : false;
    }

    public void Add(Product product)
    {
        _db.Products.Add(new Product
        {
            Id = product.Id,
            Name = product.Name,
            Order = product.Order,
            Price = product.Price,
            ImageUrl = product.ImageUrl,
            BrandId = product.BrandId,
            SectionId = product.SectionId,
        });
        _db.SaveChanges();
    }
}