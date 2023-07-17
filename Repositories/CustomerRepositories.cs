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
    public class CustomerRepositories : ICustomerRepositories
    {
        public int CountPagingCustomer(int pageIndex, int pageSize, string textSearch, string status, string dateFrom, string dateTo, string typeCustomer, int wahoId) => CustomerDAO.CountPagingCustomer(pageIndex, pageSize, textSearch, status, dateFrom, dateTo, typeCustomer, wahoId);
        public List<Customer> GetCustomersPagingAndFilter(int pageIndex, int pageSize, string textSearch, string status, string dateFrom, string dateTo, string typeCustomer, int wahoId) => CustomerDAO.GetCustomersPagingAndFilter(pageIndex, pageSize, textSearch, status, dateFrom, dateTo, typeCustomer, wahoId);
        public string SaveCustomer(PostCustomerVM employeeVM) => CustomerDAO.SaveCustomer(employeeVM);
        public Customer FindCustomerById(int id) => CustomerDAO.FindCustomerById(id);
        public string UpdateCustomer(PostCustomerVM employeeVM) => CustomerDAO.UpdateCustomer(employeeVM);
    }
}
