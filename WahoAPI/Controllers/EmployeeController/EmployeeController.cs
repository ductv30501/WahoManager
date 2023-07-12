using BusinessObjects.WahoModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using ViewModels.EmployeeViewModels;

namespace WahoAPI.Controllers.EmployeeController
{
    [Route("waho/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private IEmployeeRepositories respository = new EmployeeRepositories();
        [HttpGet("login")]
        public ActionResult<EmployeeVM> SearchProduct(string username, string password)
        {
            var employee = respository.GetEmployeeByUsernamePassword(username, password);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }
        [HttpGet("countPagingEmployee")]
        public ActionResult<int> countPagingEmployee(string? textSearch, string status, string title, int wahoId)
        {
            var total = respository.countPagingEmployee(textSearch, status,title, wahoId);
            if (total == null)
            {
                return NotFound();
            }
            return Ok(total);
        }
        [HttpGet("getEmployeePaging")]
        public ActionResult<List<Employee>> getEmployeePaging(int pageIndex, int pageSize, string? textSearch, string title, string status, int wahoId)
        {
            var employees = respository.getEmployeePaging(pageIndex, pageSize, textSearch, title, status, wahoId);
            if (employees == null || employees.Count == 0)
            {
                return NotFound();
            }
            return Ok(employees);
        }
        [HttpPost]
        public IActionResult PostEmployee(PostEmployeeVM pe)
        {
            string message = respository.SaveEmployee(pe);
            return Ok(message);
        }
        [HttpGet("username")]
        public ActionResult<Employee> findEmployeeByUsername(string username, int wahoId)
        {
            Employee employee = respository.findEmployeeByUsername(username, wahoId);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }
        [HttpGet("usernameAllWaho")]
        public ActionResult<Employee> findEmployeeByUsernameAll(string username)
        {
            Employee employee = respository.findEmployeeByUsernameAll(username);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }
        [HttpPut]
        public IActionResult PutEmployee(PostCustomerVM pe)
        {
            string message = respository.updateEmployee(pe);
            return Ok(message);
        }
        [HttpGet("EmployeesInWaho")]
        public ActionResult<List<Employee>> getEmployeeInWaho(int wahoId)
        {
            List<Employee> employees = respository.GetEmployeesInWaho(wahoId);
            if (employees.Count == 0)
            {
                return NotFound();
            }
            return Ok(employees);
        }
        [HttpGet("EmployeesInWahoByRole")]
        public ActionResult<List<Employee>> EmployeesInWahoByRole(int role,int wahoId)
        {
            List<Employee> employees = respository.GetEmployeesInWahoByRole(role, wahoId);
            if (employees.Count == 0)
            {
                return NotFound();
            }
            return Ok(employees);
        }
        [HttpGet("GetAll")]
        public ActionResult<List<Employee>> GetAll(int wahoId)
        {
            List<Employee> employees = respository.GetAllEmployeesInWaho(wahoId);
            return Ok(employees);
        }
        [HttpGet("GetByEmail")]
        public ActionResult<Employee> GetByEmail(string email)
        {
            Employee employee = respository.GetEmployeesByEmail(email);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }
    }
}
