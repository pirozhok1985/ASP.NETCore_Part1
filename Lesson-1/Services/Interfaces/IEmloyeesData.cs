using WebStore.Models;

namespace WebStore.Services.Interfaces;

public interface IEmployeesData
{
    public IEnumerable<Employee> GetAllEmployees();
    public Employee? GetEmployeeById(int id);
    public void Add(Employee employee);
    public void Edit(Employee employee);
    public void Delete(int id);
}

