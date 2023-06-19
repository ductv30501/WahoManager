using BusinessObjects.WahoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.CustomerViewModels;

namespace Repositories.CustomerRepository
{
    public interface ICustomerRepository
    {
        int SaveCustomer(CustomerVM customerVM);
        List<Customer> GetCustomersSearch(string textSearch, int wahoId);
    }
}
