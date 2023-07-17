using BusinessObjects.WahoModels;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.CustomerViewModels;

namespace Repositories
{
    public interface ICustomerRepositories
    {
        public int CountPagingCustomer(int pageIndex, int pageSize, string textSearch, string status, string dateFrom, string dateTo, string typeCustomer, int wahoId);
        public List<Customer> GetCustomersPagingAndFilter(int pageIndex, int pageSize, string textSearch, string status, string dateFrom, string dateTo, string typeCustomer, int wahoId);
        public string SaveCustomer(PostCustomerVM employeeVM);
        public Customer FindCustomerById(int id);
        public string UpdateCustomer(PostCustomerVM employeeVM);
    }
}
