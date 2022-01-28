using Microsoft.AspNetCore.Mvc;
using WebStore.DAL.Migrations;
using WebStore.Interfaces.Services;
using Employee = WebStore.Domain.Entities.Employee;

namespace WebStore.WebApi.Controllers;

[ApiController]
[Route("api/employees")] //http://localhost:5071/api/employees
public class EmployeesApiController : Controller
{
    private readonly IEmployeesData _employeesData;

    public EmployeesApiController(IEmployeesData employeesData) => _employeesData = employeesData;

    [HttpGet]
    public IActionResult Get()
    {
        var employees = _employeesData.GetAllEmployees();
        return Ok(employees);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var employee = _employeesData.GetEmployeeById(id);
        if (employee is null)
            return NotFound();
        return Ok(employee);
    }

    [HttpPost]
    public IActionResult Add(Employee employee)
    {
        var id = _employeesData.Add(employee);
        return CreatedAtAction(nameof(GetById), new {Id = id},employee);
    }

    [HttpPut]
    public IActionResult Update(Employee employee)
    {
        _employeesData.Edit(employee);
        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _employeesData.Delete(id);
        return Ok();
    }
}