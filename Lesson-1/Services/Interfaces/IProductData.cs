﻿using WebStore.Domain.Entities;

namespace WebStore.Services.Interfaces;

public interface IProductData
{
    public IEnumerable<Brand> GetBrands();
    public IEnumerable<Section> GetSections();
}
