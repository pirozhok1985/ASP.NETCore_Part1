using WebStore.Data;
using WebStore.Models;
using WebStore.Services.Interfaces;

namespace WebStore.Services.Memory;

public class EmployeeDataInMemory : IEmployeesData
{
    private readonly ICollection<Employee> _Employees;
    private ILogger<EmployeeDataInMemory> _Logger;
    public EmployeeDataInMemory(ILogger<EmployeeDataInMemory> logger)
    {
        _Employees = TestData.Employees;
        _Logger = logger;
    }
    public int Add(Employee employee)
    {
        if (employee == null)
            throw new ApplicationException();
        if (_Employees.Contains(employee))
            return employee.Id;
        employee.Id = _Employees.DefaultIfEmpty().Max(e => e?.Id ?? 0) + 1;
        _Employees.Add(employee);
        _Logger.LogInformation("Был добавлен пользователь с id {0}", employee.Id);
        return employee.Id;
    }

    public void Delete(int id)
    {
        var employee = GetEmployeeById(id);
        if (employee != null)
            _Employees.Remove(employee);
        _Logger.LogWarning("Был удалён пользователь с id {0}", id);
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
        _Logger.LogWarning("Был отредактирован пользователь с id {0}", employee.Id);
    }

    public IEnumerable<Employee> GetAllEmployees() => _Employees;


    public Employee? GetEmployeeById(int id) => _Employees.FirstOrDefault(e => e.Id == id);

}

