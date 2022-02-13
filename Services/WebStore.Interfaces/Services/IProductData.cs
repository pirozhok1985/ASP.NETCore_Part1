﻿using WebStore.Domain;
using WebStore.Domain.Entities;

namespace WebStore.Interfaces.Services;

public interface IProductData
{
    public IEnumerable<Brand>? GetBrands();
    public IEnumerable<Section>? GetSections();
    public ProductsPage GetProducts(ProductFilter? filter = null);
    public Product? GetProductById(int id);
    public Section? GetSectionById(int? id);
    public Brand? GetBrandById(int id);
    public void Edit(Product product);
    public bool Delete(int id);
    public void Add(Product product);
}

