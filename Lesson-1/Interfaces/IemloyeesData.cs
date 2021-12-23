using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Models;

namespace WebStore.Interfaces
{
    public interface IemloyeesData
    {
        public IEnumerable<Employee> GetAllEmployees();
        public Employee GetEmployeeById(int id);
        public void Add(Employee employee);
        public void Edit(Employee employee);
        public void Delete(int id);
    }
}
