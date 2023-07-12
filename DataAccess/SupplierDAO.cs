using AutoMapper;
using BusinessObjects.WahoModels;
using DataAccess.AutoMapperConfig;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.EmployeeViewModels;
using ViewModels.SupplierViewModels;

namespace DataAccess
{
    public class SupplierDAO
    {
        private static readonly IMapper _mapper = SupplierMapper.ConfigureVMToM();
        public static void addSupplier(SupplierVM supplierVM)
        {
            Supplier supplier = _mapper.Map<Supplier>(supplierVM);
            try
            {
                using (var context = new WahoS8Context())
                {
                    context.Suppliers.Add(supplier);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static Supplier getsupplierByID(int id)
        {
            try
            {
                using (var context = new WahoS8Context())
                {
                    Supplier supplier = new Supplier();
                    supplier = context.Suppliers.FirstOrDefault(s => s.SupplierId == id && s.Active == true);
                    return supplier;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static void updateSupplier(SupplierVM supplierVM)
        {
            Supplier supplier = _mapper.Map<Supplier>(supplierVM);
            try
            {
                using (var context = new WahoS8Context())
                {
                    context.Entry<Supplier>(supplier).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static List<Supplier> GetSupplies(int wahoId)
        {
            List<Supplier> suppliers = new List<Supplier>();
            try
            {
                using (var _context = new WahoS8Context())
                {
                    suppliers = _context.Suppliers.Where(s => s.WahoId == wahoId && s.Active == true).ToList();
                    return suppliers;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static int countSuppliers(string textSearch, int wahoId)
        {
            int count = 0;
            try
            {
                using (var context = new WahoS8Context())
                {
                    var query = context.Suppliers.AsQueryable();
                    if (!string.IsNullOrEmpty(textSearch))
                    {
                        query = query.Where(s => s.Branch.ToLower().Contains(textSearch) || s.Address.ToLower().Contains(textSearch) || s.CompanyName.ToLower().Contains(textSearch) || s.Phone.ToLower().Contains(textSearch)
                            || s.TaxCode.ToLower().Contains(textSearch));
                    }

                    count = query.Where(s => s.Active == true && s.WahoId == wahoId)
                     .Count();
                    return count;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            
        }
        public static List<Supplier> GetSupplierPagingAndFilter(int pageIndex, int pageSize, string textSearch, int wahoId)
        {
            List<Supplier> suppliers = new List<Supplier>();
            try
            {
                using (var _context = new WahoS8Context())
                {
                    
                    var query = _context.Suppliers.Where(s => s.Active == true && s.WahoId == wahoId).AsQueryable();
                    if (!string.IsNullOrEmpty(textSearch))
                    {
                        query = query.Where(s => s.Branch.ToLower().Contains(textSearch.ToLower()) || s.Address.ToLower().Contains(textSearch.ToLower()) 
                                    || s.CompanyName.ToLower().Contains(textSearch.ToLower()) || s.Phone.ToLower().Contains(textSearch.ToLower())
                                    || s.TaxCode.ToLower().Contains(textSearch.ToLower()));
                    }
                    suppliers = query.OrderBy(s => s.SupplierId)
                                 .Skip((pageIndex - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToList();
                    return suppliers;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
