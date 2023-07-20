using BusinessObjects.WahoModels;
using DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using ViewModels.CustomerViewModels;

namespace WahoAPI.Controllers.CustomerController
{
    [Route("waho/[controller]")]
    [ApiController]

    public class CustomerController : ControllerBase
    {
        private ICustomerRepositories respository = new CustomerRepositories();
        [HttpGet("getCustomers")]
        [Authorize(Roles = "1")]

        public ActionResult<Customer> GetCustomersPagingAndFilter(int pageIndex, int pageSize, string? textSearch, string? status, string? dateFrom, string? dateTo, string? typeCustomer, int wahoId)
        {
            var customer = respository.GetCustomersPagingAndFilter(pageIndex, pageSize, textSearch, status, dateFrom, dateTo, typeCustomer, wahoId);

            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }
        [HttpGet("count")]
        [Authorize(Roles = "1")]

        public ActionResult<int> CountPagingCustomer(int pageIndex, int pageSize, string? textSearch, string? status, string? dateFrom, string? dateTo, string? typeCustomer, int wahoId)
        {
            var total = respository.CountPagingCustomer(pageIndex, pageSize, textSearch, status, dateFrom, dateTo, typeCustomer, wahoId);
            if (total == null)
            {
                return NotFound();
            }
            return Ok(total);
        }
        
        [HttpPost]
        [Authorize]

        public IActionResult PostCustomer(PostCustomerVM pe)
        {
            string message = respository.SaveCustomer(pe);
            return Ok(message);
        }
        [HttpGet("detail")]
        [Authorize]

        public ActionResult<Employee> FindCustomerById(int id)
        {
            Customer customer = respository.FindCustomerById(id);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }
        [HttpPut]
        [Authorize]

        public IActionResult PutCustomer(PostCustomerVM pe)
        {
            string message = respository.UpdateCustomer(pe);
            return Ok(message);
        }
    }
}
