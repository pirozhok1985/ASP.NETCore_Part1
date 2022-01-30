namespace WebStore.Interfaces.TestAPI;

public interface IValueService
{
    IEnumerable<string>? GetValues();
    string? GetById(int id);
    int Count();
    void Add(string value);
    void Edit(int id, string value);
    void Delete(int id);
}