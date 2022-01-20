using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Identity;
using WebStore.Domain.Entities;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Controllers;

[Route("Staff/{action=Index}/{id?}"),Authorize]
public class EmployeeController : Controller
{
    private IEmployeesData _Data;

    public EmployeeController(IEmployeesData data)
    {
        _Data = data;
    }
    public IActionResult Index()
    {
        return View(_Data.GetAllEmployees());
    }
    public IActionResult Details(int id)
    {
        var employee = _Data.GetEmployeeById(id);
        if (employee == null)
            return NotFound();
        return View(employee);
    }

    [Authorize(Roles = Role.Administrators)]
    public IActionResult Edit(int id)
    {
        var employee = _Data.GetEmployeeById(id);
        if (employee == null)
            return NotFound();
        var employeeEdit = new EmployeeViewModel
        {
            Id = employee.Id,
            Name = employee.FirstName,
            LastName = employee.SecondName,
            Patronymic = employee.Patronymic,
            Age = employee.Age,
            Income = employee.Income
        };
        return View(employeeEdit);
    }
    [HttpPost,Authorize(Roles = Role.Administrators)]
    public IActionResult Edit(EmployeeViewModel emp)
    {
        if (!ModelState.IsValid)
            return View(emp);

        var employee = new Employee
        {
            Id = emp.Id,
            FirstName = emp.Name,
            SecondName = emp.LastName,
            Age = emp.Age,
            Patronymic = emp.Patronymic,
            Income = emp.Income
        };
        _Data.Edit(employee);
        return RedirectToAction("Index");
    }

    [Authorize(Roles = Role.Administrators)]
    public IActionResult Delete(int id)
    {
        var employee = _Data.GetEmployeeById(id);
        if (employee == null)
            return NotFound();
        return View(employee);
    }
    [HttpPost, Authorize(Roles = Role.Administrators)]
    public IActionResult Delete(Employee employee)
    {
        _Data.Delete(employee.Id);
        return RedirectToAction("Index");
    }

    [Authorize(Roles = Role.Administrators)]
    public IActionResult Add()
    {
        return View(new EmployeeViewModel());
    }

    [HttpPost, Authorize(Roles = Role.Administrators)]
    public IActionResult Add(EmployeeViewModel emp)
    {
        if (!ModelState.IsValid)
            return View(emp);

        var employee = new Employee
        {
            Id = emp.Id,
            FirstName = emp.Name,
            SecondName = emp.LastName,
            Age = emp.Age,
            Patronymic = emp.Patronymic,
            Income = emp.Income
        };
        _Data.Add(employee);
        return RedirectToAction("Index");
    }
}