using WebStore.Interfaces.TestAPI;

namespace WebStore.WebAPI.Clients.Values;

public class ValuesClient : IValueService
{
    private HttpClient _Client;
    public IEnumerable<string> GetValues()
    {
        throw new NotImplementedException();
    }

    public string GetById(int id)
    {
        throw new NotImplementedException();
    }

    public int Count()
    {
        throw new NotImplementedException();
    }

    public void Add(string value)
    {
        throw new NotImplementedException();
    }

    public void Edit(int id)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }
}