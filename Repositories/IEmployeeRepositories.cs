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
    }
}
