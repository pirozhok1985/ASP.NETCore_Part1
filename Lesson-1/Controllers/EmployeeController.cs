using Microsoft.AspNetCore.Mvc;
using WebStore.Data;
using WebStore.Models;
using WebStore.Services;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    [Route("Staff/{action=Index}/{id?}")]
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
            Employee employee = _Data.GetEmployeeById(id);
            if (employee == null)
                return NotFound();
            return View(employee);
        }
        public IActionResult Edit(int id)
        {
            Employee employee = _Data.GetEmployeeById(id);
            if (employee == null)
                return NotFound();
            var employeeEdit = new EmployeeViewModel
            {
                Id = employee.Id,
                Name = employee.FirstName,
                LastName = employee.SecondName,
                Patronymic = employee.Patronymic,
                Age = employee.Age,
                Income = employee.Income,
            };
            return View(employeeEdit);
        }
        [HttpPost]
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
        public IActionResult Delete(int id)
        {
            Employee employee = _Data.GetEmployeeById(id);
            if (employee == null)
                return NotFound();
            return View(employee);
        }
        [HttpPost]
        public IActionResult Delete(Employee employee)
        {
            _Data.Delete(employee.Id);
            return RedirectToAction("Index");
        }

        public IActionResult Add() => View(new EmployeeViewModel());

        [HttpPost]
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
}
