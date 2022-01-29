using System.Net.Http.Json;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Employees;

public class EmployeesClient : BaseClient, IEmployeesData
{
    public EmployeesClient(HttpClient client) : base(client, "api/employees")
    {
    }

    public IEnumerable<Employee>? GetAllEmployees() => Get<IEnumerable<Employee>>(Address);

    public Employee? GetEmployeeById(int id) => Get<Employee>($"{Address}/{id}");

    public int Add(Employee employee)
    {
        var result = Post(Address, employee);
        if (result is null)
            return -1;
        var desResult = result.Content.ReadFromJsonAsync<Employee>();
        employee.Id = desResult.Id;
        return desResult.Id;
    }

    public void Edit(Employee employee)
    {
        var response = Put(Address, employee);
    }

    public bool Delete(int id)
    {
        var response = Delete($"{Address}/{id}");
        return response.IsSuccessStatusCode;
    }
}