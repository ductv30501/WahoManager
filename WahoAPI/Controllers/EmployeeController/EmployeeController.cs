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
    }
}
