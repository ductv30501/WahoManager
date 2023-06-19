using AutoMapper;
using BusinessObjects.WahoModels;
using DataAccess.AutoMapperConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.CustomerViewModels;

namespace DataAccess
{
    public class CustomersDAO
    {
        private static readonly IMapper _mapper = CustomerMapper.ConfigureVMtoM();
        public static int SaveCustomer(CustomerVM customerVM)
        {
            Customer customer = _mapper.Map<Customer>(customerVM);
            try
            {
                using (var _context = new WahoS8Context())
                {
                    _context.Customers.Add(customer);
                    _context.SaveChanges();
                    return customer.CustomerId;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static List<Customer> GetCustomersSearch(string textSearch, int wahoId)
        {
            List<Customer> Customers = new List<Customer>();
            try
            {
                using (var _context = new WahoS8Context())
                {
                    var query = _context.Customers.AsQueryable();
                    if (!string.IsNullOrEmpty(textSearch))
                    {
                        query = query.Where(p => p.CustomerName.ToLower().Contains(textSearch.ToLower()) || p.Phone.Contains(textSearch));
                    }
                    Customers = query.Where(p => p.Active == true && p.WahoId == wahoId)
                    .Take(5).ToList();
                    return Customers;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
