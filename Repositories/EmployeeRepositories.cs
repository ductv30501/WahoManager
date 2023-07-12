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
        public int countPagingEmployee(string textSearch, string status, string title, int wahoId) => EmployeeDAO.countPagingEmployee(textSearch, status, title, wahoId);
        public List<Employee> getEmployeePaging(int pageIndex, int pageSize, string textSearch, string title, string status, int wahoId) => EmployeeDAO.getEmployeePaging(pageIndex, pageSize, textSearch, title, status, wahoId);
        public EmployeeVM GetEmployeeByUsernamePassword(string username, string password) => EmployeeDAO.GetEmployeeByUserAndPass(username,password);

        public string SaveEmployee(PostEmployeeVM employeeVM) => EmployeeDAO.SaveEmployee(employeeVM);
        public Employee findEmployeeByUsername(string username, int wahoId) => EmployeeDAO.FindEmployeeByUsername(username, wahoId);
        public Employee findEmployeeByUsernameAll(string username) => EmployeeDAO.FindEmployeeByUsernameAll(username);
        public string updateEmployee(PostEmployeeVM employeeVM) => EmployeeDAO.updateEmployee(employeeVM);

        public List<Employee> GetEmployeesInWaho(int wahoId) => EmployeeDAO.GetEmployeesInWaho(wahoId);

        public List<Employee> GetEmployeesInWahoByRole(int role, int wahoId) => EmployeeDAO.GetEmployeesInWahoByRole(role, wahoId);

        public List<Employee> GetAllEmployeesInWaho(int wahoId)
            => EmployeeDAO.GetAllEmployeesInWaho(wahoId);

        public Employee GetEmployeesByEmail(string email)
            => EmployeeDAO.GetEmployeesByEmail(email);
    }
}
