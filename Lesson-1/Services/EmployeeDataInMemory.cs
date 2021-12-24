using WebStore.Data;
using WebStore.Models;
using WebStore.Services.Interfaces;

namespace WebStore.Services;

public class EmployeeDataInMemory : IEmployeesData
{
    private readonly ICollection<Employee> _Employees;
    public EmployeeDataInMemory()
    {
        _Employees = TestData.Employees;
    }
    public void Add(Employee employee)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public void Edit(Employee employee)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Employee> GetAllEmployees() => _Employees;


    public Employee? GetEmployeeById(int id) => _Employees.FirstOrDefault(e => e.Id == id);

}

