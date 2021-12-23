using Microsoft.AspNetCore.Mvc;
using WebStore.Data;
using WebStore.Models;

namespace WebStore.Controllers
{
    [Route("Staff/{action=Index}/{id?}")]
    public class EmployeeController : Controller
    {
        private readonly List<Employee> __Employees;

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
            return View(employee);
        }
        [HttpPost]
        public IActionResult Edit(Employee employee)
        {
            var index = __Employees.FindIndex(e => e.Id == employee.Id);
            __Employees.RemoveAt(index);
            __Employees.Insert(index, employee);

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
