using Microsoft.Extensions.Logging;
using WebStore.DAL.Context;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.Services.Services.Database;

public class EmployeeDataDb : IEmployeesData
{
    private readonly WebStoreDB _db;
    private readonly ILogger<EmployeeDataDb> _logger;

    public EmployeeDataDb(WebStoreDB db, ILogger<EmployeeDataDb> logger)
    {
        _db = db;
        _logger = logger;
    }

    public IEnumerable<Employee>? GetAllEmployees() => _db.Employees;

    public Employee? GetEmployeeById(int id) => _db.Employees?.Find(id);


    public int Add(Employee employee)
    {
        _db.Employees?.Add(employee);
        _db.SaveChanges();
        return employee.Id;
    }

    public void Edit(Employee employee)
    {
        _db.Employees?.Update(employee);
        _db.SaveChanges();
    }

    public bool Delete(int id)
    {
        var employee = _db.Employees!.FirstOrDefault(e => e.Id == id);
        _db.Employees!.Remove(employee!);
        var result = _db.SaveChanges();
        return result != 0 ? true : false;
    }
}