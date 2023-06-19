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
    public class EmployeeRepositories : IEmployeeRepositories
    {
        public int countPagingEmployee(string textSearch, string status, string title) => EmployeeDAO.countPagingEmployee(textSearch, status, title);
        public List<Employee> getEmployeePaging(int pageIndex, int pageSize, string textSearch, string title, string status) => EmployeeDAO.getEmployeePaging(pageIndex, pageSize, textSearch, title, status);
        public EmployeeVM GetEmployeeByUsernamePassword(string username, string password) => EmployeeDAO.GetEmployeeByUserAndPass(username,password);
        public string SaveEmployee(PostCustomerVM employeeVM) => EmployeeDAO.SaveEmployee(employeeVM);
        public Employee findEmployeeByUsername(string username) => EmployeeDAO.FindEmployeeByUsername(username);
        public string updateEmployee(PostCustomerVM employeeVM) => EmployeeDAO.updateEmployee(employeeVM);
    }
}
