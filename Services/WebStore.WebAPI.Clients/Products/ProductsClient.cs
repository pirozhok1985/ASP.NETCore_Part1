using System.Net.Http.Json;
using Microsoft.VisualBasic;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Products;

public class ProductsClient : BaseClient, IProductData
{
    public ProductsClient(HttpClient client) : base(client, "api/products")
    {
    }

    public IEnumerable<Brand>? GetBrands() => Get<IEnumerable<Brand>>($"{Address}/Brands");

    public IEnumerable<Section>? GetSections() => Get<IEnumerable<Section>>($"{Address}/Sections");


    public IEnumerable<Product> GetProducts(ProductFilter? filter)
    {
        var result = Post(Address, filter ?? new ());
        var products = result.Content.ReadFromJsonAsync<IEnumerable<Product>>().Result;
        return products;
    }

    public Product? GetProductById(int id) => Get<Product>($"{Address}/{id}");
 
    public void Edit(Product product) => Put(Address, product);

    public bool Delete(int id)
    {
        var result = Delete($"{Address}/{id}");
        return result.IsSuccessStatusCode;
    }

    public void Add(Product product) => Post(Address, product);

}