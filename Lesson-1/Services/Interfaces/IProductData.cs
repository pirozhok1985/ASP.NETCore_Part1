using WebStore.Domain.Entities;

namespace WebStore.Services.Interfaces;

public interface IProductData
{
    public IEnumerable<Brand> GetBrand();
    public IEnumerable<Section> GetSections();
}

