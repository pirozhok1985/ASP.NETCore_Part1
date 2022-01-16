using WebStore.DAL.Context;
using WebStore.Domain.Entities;
using WebStore.Services.Interfaces;

namespace WebStore.Services.Database;

public class EmployeeDataDB : IEmployeesData
{
    private readonly WebStoreDB _db;
    private readonly ILogger<EmployeeDataDB> _logger;

    public EmployeeDataDB(WebStoreDB db, ILogger<EmployeeDataDB> logger)
    {
        _db = db;
        _logger = logger;
    }

    public IEnumerable<Employee> GetAllEmployees() => _db.Employees;

    public Employee? GetEmployeeById(int id) => _db.Employees.Find(id);


    public int Add(Employee employee)
    {
        _db.Employees.Add(employee);
        _db.SaveChanges();
        return employee.Id;
    }

    public void Edit(Employee employee)
    {
        _db.Employees.Update(employee);
        _db.SaveChanges();
    }

    public void Delete(int id)
    {
        var employee = _db.Employees.Select(e => new Employee() {Id = id}).FirstOrDefault(e => e.Id == id);
        _db.Employees.Remove(employee);
    }
}