using BusinessObjects.WahoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.EmployeeViewModels;

namespace DataAccess
{
    public class EmployeeDAO
    {
        public static EmployeeVM GetEmployeeByUserAndPass(string userName, string password)
        {
            try
            {
                using (var context = new WahoS8Context())
                {
                    EmployeeVM employeevm = (from e in context.Employees
                                     where e.UserName == userName && e.Password == password
                                     select new EmployeeVM
                                     {
                                         UserName= e.UserName,
                                         Password= e.Password,
                                         Role = e.Role,
                                         Email= e.Email,
                                         Active= e.Active,
                                         EmployeeName= e.EmployeeName,
                                         WahoId= e.WahoId
                                     }).FirstOrDefault();
                    return employeevm;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
