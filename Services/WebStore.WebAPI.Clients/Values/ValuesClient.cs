using System.Net.Http.Json;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.TestAPI;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Values;

public class ValuesClient : BaseClient, IValueService
{
    public ValuesClient(HttpClient client):base(client, "api/values")
    {
        
    }
   
    public IEnumerable<string> GetValues()
    {
        var response = Http.GetAsync(Address).Result;
        return response.IsSuccessStatusCode ? response.Content.ReadFromJsonAsync<IEnumerable<string>>().Result : Enumerable.Empty<string>();
    }

    public string GetById(int id)
    {
        var response = Http.GetAsync($"{Address}/{id}").Result;
        return response.IsSuccessStatusCode ? response.Content.ReadFromJsonAsync<string>().Result : String.Empty;
    }

    public int Count()
    {
        var response = Http.GetAsync($"{Address}/count").Result;
        return response.IsSuccessStatusCode ? response.Content.ReadFromJsonAsync<int>().Result : -1;
    }

    public void Add(string value)
    {
        var response = Http.PostAsJsonAsync(Address, value).Result;
        response.EnsureSuccessStatusCode();
    }
    
    public void Edit(int id, string value)
    {
        var response = Http.PostAsJsonAsync($"{Address}/{id}", value).Result;
        response.EnsureSuccessStatusCode();
    }

    public void Delete(int id)
    {
        var response = Http.DeleteAsync($"{Address}/{id}").Result;
        response.EnsureSuccessStatusCode();
    }
}