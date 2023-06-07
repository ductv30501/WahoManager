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
        public ActionResult<int> countPagingEmployee(string? textSearch, string status, string title)
        {
            var total = respository.countPagingEmployee(textSearch, status,title);
            if (total == null)
            {
                return NotFound();
            }
            return Ok(total);
        }
        [HttpGet("getEmployeePaging")]
        public ActionResult<List<Employee>> getEmployeePaging(int pageIndex, int pageSize, string? textSearch, string title, string status)
        {
            var employees = respository.getEmployeePaging(pageIndex, pageSize, textSearch, title, status);
            if (employees == null || employees.Count == 0)
            {
                return NotFound();
            }
            return Ok(employees);
        }
        [HttpPost]
        public IActionResult PostOrder(PostEmployeeVM pe)
        {
            string message = respository.SaveEmployee(pe);
            return Ok(message);
        }
        [HttpGet("username")]
        public ActionResult<Employee> findEmployeeByUsername(string username)
        {
            Employee employee = respository.findEmployeeByUsername(username);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }
        [HttpPut]
        public IActionResult PutEmployee(PostEmployeeVM pe)
        {
            string message = respository.updateEmployee(pe);
            return Ok(message);
        }
    }
}
