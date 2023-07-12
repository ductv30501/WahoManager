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
    public class CustomerDAO
    {
        private static readonly IMapper _mapper = customerMapper.ConfigureVMtoM();
        private static readonly IMapper _mapperGet = customerMapper.ConfigureLMtoLVM();

        public static int CountPagingCustomer(int pageIndex, int pageSize, string textSearch, string status, string dateFrom, string dateTo, string typeCustomer, int wahoId)
        {
            try
            {
                using (var context = new WahoS8Context())
                {
                    var query = context.Customers.Where(c => c.WahoId == wahoId).AsQueryable();
                    if (!string.IsNullOrEmpty(textSearch))
                    {
                        query = query.Where(c => (c.CustomerName.Contains(textSearch)
                                         || c.Phone.Contains(textSearch)
                                         || c.Email.Contains(textSearch)
                                         || c.TaxCode.Contains(textSearch)));
                    }
                    if (status != "all")
                    {
                        query = query.Where(c => c.Active.ToString().Contains(status));
                    }
                    if (typeCustomer != "all")
                    {
                        query = query.Where(c => (c.TypeOfCustomer.ToString().Contains(typeCustomer)));
                    }
                    if (!string.IsNullOrEmpty(dateFrom))
                    {
                        query = query.Where(c => (c.Dob >= DateTime.Parse(dateFrom) && c.Dob <= DateTime.Parse(dateTo)));
                    }

                    return query.Count();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static List<GetCustomerVM> GetCustomersPagingAndFilter(int pageIndex, int pageSize, string textSearch, string status, string dateFrom, string dateTo, string typeCustomer, int wahoId)
        {

            List<Customer> customers = new List<Customer>();
            using (var context = new WahoS8Context())
            {
                //default 
                var query = from c in context.Customers.Where(c => c.WahoId == wahoId) select c;

                if (!string.IsNullOrEmpty(textSearch.Trim()))
                {
                    query = query.Where(c => (c.CustomerName.Contains(textSearch)
                                     || c.Phone.Contains(textSearch)
                                     || c.Email.Contains(textSearch)
                                     || c.TaxCode.Contains(textSearch)));
                }

                if (status != "all")
                {
                    query = query.Where(c => (c.Active.ToString().Contains(status)));
                }

                if (typeCustomer != "all")
                {
                    query = query.Where(c => (c.TypeOfCustomer.ToString().Contains(typeCustomer)));
                }

                if (!string.IsNullOrEmpty(dateFrom))
                {
                    query = query.Where(c => (c.Dob >= DateTime.Parse(dateFrom) && c.Dob <= DateTime.Parse(dateTo)));
                }

                customers = query
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                return _mapperGet.Map<List<GetCustomerVM>>(customers);
            }
        }

        public static string SaveCustomer(PostCustomerVM customerVM)
        {
            Customer customer = _mapper.Map<Customer>(customerVM);
            try
            {
                using (var context = new WahoS8Context())
                {
                    context.Customers.Add(_mapper.Map<Customer>(customerVM));
                    context.SaveChanges();
                    return $"Thêm thành công khách hàng mới: {customer.CustomerName}";
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static Customer FindCustomerById(int id, int wahoId)
        {
            Customer customer = new Customer();
            try
            {
                using (var context = new WahoS8Context())
                {
                    customer = context.Customers.FirstOrDefault(e => e.CustomerId == id && e.WahoId == wahoId);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return customer;
        }
        public static string UpdateCustomer(PostCustomerVM customerVM)
        {
            try
            {
                using (var context = new WahoS8Context())
                {
                    Customer customer = new Customer();
                    customer = _mapper.Map<Customer>(customerVM);
                    context.Entry<Customer>(customer).State = EntityState.Modified;
                    context.SaveChanges();
                    return $"dã cập nhật thông tin của {customer.CustomerName} thành công";
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
