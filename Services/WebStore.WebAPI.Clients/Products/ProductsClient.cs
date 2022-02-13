using System.Net.Http.Json;
using Microsoft.VisualBasic;
using WebStore.Domain;
using WebStore.Domain.DTO;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using WebStore.Services;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Products;

public class ProductsClient : BaseClient, IProductData
{
    public ProductsClient(HttpClient client) : base(client, WebAddresses.Products)
    {
    }

    public IEnumerable<Brand>? GetBrands() => Get<IEnumerable<BrandDto>>($"{Address}/Brands")!.FromDto()!;

    public IEnumerable<Section>? GetSections() => Get<IEnumerable<SectionDto>>($"{Address}/Sections")!.FromDto()!;


    public ProductsPage GetProducts(ProductFilter? filter)
    {
        var result = Post(Address, filter ?? new ());
        var products = result!.Content.ReadFromJsonAsync<ProductsPageDto>().Result;
        return products!.FromDto();
    }

    public Product? GetProductById(int id) => Get<ProductDto>($"{Address}/{id}").FromDto();
    public Section? GetSectionById(int? id) => Get<SectionDto>($"{Address}/Sections/{id}").FromDto();

    public Brand? GetBrandById(int id) => Get<BrandDto>($"{Address}/Brands/{id}").FromDto();

    public void Edit(Product product) => Put(Address, product.ToDto());

    public bool Delete(int id)
    {
        var result = Delete($"{Address}/{id}");
        return result.IsSuccessStatusCode;
    }

    public void Add(Product product) => Post($"{Address}/new", product.ToDto());

}