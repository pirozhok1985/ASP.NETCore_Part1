﻿using Microsoft.AspNetCore.Mvc;
using WebStore.Data;
using WebStore.Models;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    [Route("Staff/{action=Index}/{id?}")]
    public class EmployeeController : Controller
    {
        private readonly ICollection<Employee> __Employees;

        public EmployeeController()
        {
            __Employees = TestData.Employees;
        }
        public IActionResult Index()
        {
            return View(__Employees);
        }
        public IActionResult Details(int id)
        {
            Employee employee = __Employees.SingleOrDefault(e => e.Id == id);
            if (employee == null)
                return NotFound();
            return View(employee);
        }
        public IActionResult Edit(int id)
        {
            Employee employee = __Employees.SingleOrDefault(e => e.Id == id);
            if (employee == null)
                return NotFound();
            var employeeEdit = new EmployeeEditViewModel
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
        public IActionResult Edit(EmployeeEditViewModel employee)
        {
            //var index = __Employees.FindIndex(e => e.Id == employee.Id);
            //__Employees.RemoveAt(index);
            //__Employees.Insert(index, employee);

            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            Employee employee = __Employees.SingleOrDefault(e => e.Id == id);
            if (employee == null)
                return NotFound();
            return View(employee);
        }
        [HttpPost]
        public IActionResult Delete(Employee employee)
        {
            __Employees.Remove(employee);

            return RedirectToAction("Index");
        }
    }
}
