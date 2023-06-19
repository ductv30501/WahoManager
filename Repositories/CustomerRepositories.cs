using BusinessObjects.WahoModels;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.EmployeeViewModels;

namespace Repositories
{
    public class CustomerRepositories
    {
        public int countPagingCustomer(string textSearch, string status, string title) => CustomerDAO.countPagingCustomer(textSearch, status, title);
        public List<Customer> getCustomerPaging(int pageIndex, int pageSize, string textSearch, string title, string status) => CustomerDAO.getCustomerPaging(pageIndex, pageSize, textSearch, title, status);
        public CustomerVM GetCustomerByUsernamePassword(string username, string password) => CustomerDAO.GetCustomerByUserAndPass(username, password);
        public string SaveCustomer(PostCustomerVM employeeVM) => CustomerDAO.SaveCustomer(employeeVM);
        public Customer findCustomerByUsername(string username) => CustomerDAO.FindCustomerByUsername(username);
        public string updateCustomer(PostCustomerVM employeeVM) => CustomerDAO.updateCustomer(employeeVM);
    }
}
