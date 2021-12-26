using WebStore.Data;
using WebStore.Models;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Services;

public class EmployeeDataInMemory : IEmployeesData
{
    private readonly ICollection<Employee> _Employees;
    public EmployeeDataInMemory()
    {
        _Employees = TestData.Employees;
    }
    public int Add(Employee employee)
    {
        if (employee == null)
            throw new ApplicationException();
        if (_Employees.Contains(employee))
            return employee.Id;
        employee.Id = _Employees.DefaultIfEmpty().Max(e => e?.Id ?? 0) + 1;
        _Employees.Add(employee);
        return employee.Id;
    }

    public void Delete(int id)
    {
        var employee = GetEmployeeById(id);
        if (employee != null)
            _Employees.Remove(employee);
        foreach (var item in _Employees)
        {
            if (item.Id > id)
                --item.Id;
        }
    }

    public void Edit(Employee employee)
    {
        var emp = GetEmployeeById(employee.Id);
        emp.Age = employee.Age;
        emp.FirstName = employee.FirstName;
        emp.SecondName = employee.SecondName;
        emp.Patronymic = employee.Patronymic;
        emp.Income = employee.Income;
    }

    public IEnumerable<Employee> GetAllEmployees() => _Employees;


    public Employee? GetEmployeeById(int id) => _Employees.FirstOrDefault(e => e.Id == id);

}

