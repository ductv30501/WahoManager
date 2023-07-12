using AutoMapper;
using BusinessObjects.WahoModels;
using DataAccess.AutoMapperConfig;
using Microsoft.EntityFrameworkCore;
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
        private static readonly IMapper _mapper = customerMapper.Configure();
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
        public static Customer getCustomerByBillIdBillType(int billlId, int billType, int wahoId)
        {
            Customer customer = new Customer();
            try
            {
                using (var _context = new WahoS8Context())
                {
                    if (billType == 1)
                    {
                        var bill = _context.Bills.Include(c => c.Customer).FirstOrDefault(b => b.BillId == billlId && b.WahoId == wahoId);
                        if (bill != null)
                        {
                            customer = bill.Customer;
                        }
                    }
                    else
                    {
                        var order = _context.Oders.Include(c => c.Customer).FirstOrDefault(b => b.OderId == billlId && b.WahoId == wahoId);
                        if (order != null)
                        {
                            customer = order.Customer;
                        }
                    }                    
                    return customer;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
