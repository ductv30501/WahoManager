using BusinessObjects.WahoModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.CustomerRepository;
using System.Collections.Generic;
using ViewModels.CustomerViewModels;
using ViewModels.InventorySheetViewModels;

namespace WahoAPI.Controllers.CustomerController
{
    [Route("waho/[controller]")]
    [ApiController]

    public class CustomersController : ControllerBase
    {
        private ICustomerRepository respository = new CustomerRepositories();
        [HttpPost]
        [Authorize]

        public IActionResult PosttCustomer(CustomerVM Cvm)
        {
            int id = respository.SaveCustomer(Cvm);
            PostCustomerVM postCustomerVM = new PostCustomerVM();
            postCustomerVM.CustomerId = id;
            return Ok(postCustomerVM);
        }
        [HttpGet("search")]
        [Authorize]

        public ActionResult<List<Customer>> GetCustomersSearch(string? textSearch, int wahoId) {
            List<Customer> customers = respository.GetCustomersSearch(textSearch, wahoId);
            if (customers.Count == 0)
            {
                return NotFound();
            }
            return Ok(customers);
        }
        [HttpGet("searchCustomerOfBill")]
        [Authorize]

        public ActionResult<Customer> searchCustomerOfBill(int billlId, int billType, int wahoId)
        {
            Customer customers = respository.getCustomerByBillIdBillType(billlId, billType, wahoId);
            if (customers == null || customers == default)
            {
                return NotFound();
            }
            return Ok(customers);
        }
    }
}
