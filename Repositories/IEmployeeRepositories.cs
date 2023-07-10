using BusinessObjects.WahoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.EmployeeViewModels;

namespace Repositories
{
    public interface IEmployeeRepositories
    {
        EmployeeVM GetEmployeeByUsernamePassword(string username, string password);
        int countPagingEmployee(string textSearch, string status, string title, int wahoId);
        List<Employee> getEmployeePaging(int pageIndex, int pageSize, string textSearch, string title, string status, int wahoId);
        string SaveEmployee(PostEmployeeVM employeeVM);
        Employee findEmployeeByUsername(string username, int wahoId);
        Employee findEmployeeByUsernameAll(string username);
        string updateEmployee(PostEmployeeVM employeeVM);
        List<Employee> GetEmployeesInWaho(int wahoId);
        List<Employee> GetEmployeesInWahoByRole(int role,int wahoId);
        List<Employee> GetAllEmployeesInWaho(int wahoId);
        Employee GetEmployeesByEmail(string email);
    }
}
