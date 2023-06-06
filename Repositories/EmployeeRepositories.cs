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
        public EmployeeVM GetEmployeeByUsernamePassword(string username, string password) => EmployeeDAO.GetEmployeeByUserAndPass(username,password); 
    }
}
