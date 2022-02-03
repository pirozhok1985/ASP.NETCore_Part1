using Microsoft.AspNetCore.Mvc;
using WebStore.DAL.Migrations;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;
using Employee = WebStore.Domain.Entities.Employee;

namespace WebStore.WebApi.Controllers;

[ApiController]
[Route(WebApiAddress.Employees)] //http://localhost:5071/api/employees
public class EmployeesApiController : ControllerBase
{
    private readonly IEmployeesData _employeesData;

    /// <summary>
    /// Working with employees(CRUD operations)
    /// </summary>
    /// <param name="employeesData"></param>
    public EmployeesApiController(IEmployeesData employeesData) => _employeesData = employeesData;

    ///<summary>Get all employeess</summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(IEnumerable<Employee>))]
    public IActionResult Get()
    {
        var employees = _employeesData.GetAllEmployees();
        return Ok(employees);
    }

    ///<summary>Get employee by id</summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(Employee))]
    public IActionResult GetById(int id)
    {
        var employee = _employeesData.GetEmployeeById(id);
        if (employee is null)
            return NotFound();
        return Ok(employee);
    }

    ///<summary>Add new employee</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult Add(Employee employee)
    {
        var id = _employeesData.Add(employee);
        return CreatedAtAction(nameof(GetById), new {Id = id},employee);
    }

    ///<summary>Edit employee</summary>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Update(Employee employee)
    {
        _employeesData.Edit(employee);
        return Ok();
    }

    ///<summary>Delete employee by id</summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(bool))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        var result = _employeesData.Delete(id);
        return result ? Ok(true) : NotFound(false);
    }
}