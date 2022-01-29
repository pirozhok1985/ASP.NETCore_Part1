using WebStore.Domain.Entities;

namespace WebStore.Interfaces.Services;

public interface IEmployeesData
{
    public IEnumerable<Employee>? GetAllEmployees();
    public Employee? GetEmployeeById(int id);
    public int Add(Employee employee);
    public void Edit(Employee employee);
    public bool Delete(int id);
}

