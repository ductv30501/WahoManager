﻿using Waho.DataService;

using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;
using System.Text.Json;
using BusinessObjects.WahoModels;

namespace Waho.DataService
{
    public class DataServiceManager
    {
        private readonly BusinessObjects.WahoModels.WahoS8Context _context;
        public DataServiceManager(BusinessObjects.WahoModels.WahoS8Context context)
        {
            _context = context;
        }

        public Employee GetEmployeeByUserAndPass(string userName, string password)
        {
            return _context.Employees.Where(e => e.Active == true).FirstOrDefault(emp => emp.UserName == userName && emp.Password == password);
        }
        //get employee by email
        public Employee GetEmployeeByEmail(string email)
        {
            return _context.Employees.Where(e => e.Active == true).FirstOrDefault(emp => emp.Email == email);
        }
        public Employee GetEmployeeByUserName(string userName)
        {
            return _context.Employees.Where(e => e.Active == true).FirstOrDefault(emp => emp.UserName == userName);
        }
        public List<Employee> getEmployeePaging(int pageIndex, int pageSize, string textSearch,string title,string status)
        {
            List<Employee> employees = new List<Employee>();
            var query = _context.Employees.Where(s => s.Active == true || s.Active == false);
            if (!string.IsNullOrEmpty(textSearch))
            {
                query = query.Where(e => e.EmployeeName.ToLower().Contains(textSearch) || e.Email.ToLower().Contains(textSearch)
                                    || e.Dob.ToString().ToLower().Contains(textSearch) || e.Title.ToLower().Contains(textSearch)
                                    || e.Phone.ToLower().Contains(textSearch) 
                                    || e.Address.ToLower().Contains(textSearch) || e.HireDate.ToString().ToLower().Contains(textSearch)
                                    || e.Role.ToString().Contains(textSearch));
            }
            if (status != "all")
            {
                query = query.Where(c => (c.Active.ToString().Contains(status)));
            }
            if (title != "all")
            {
                query = query.Where(e => e.Role == int.Parse(title));
            }
            employees = query.OrderBy(s => s.UserName)
                         .Skip((pageIndex - 1) * pageSize)
                         .Take(pageSize)
                         .ToList();
            return employees;
        }
        public List<SubCategory> GetSubCategories(int id)
        {
            return _context.SubCategories
                    .Where(sb => sb.CategoryId == id)
                    .ToList();
        }
        public List<Product> GetProductsByCateID(int id)
        {
            return _context.Products
                            .Where(p => p.SubCategory.CategoryId == id)
                            .Where(p => p.Active == true)
                            .Include(p => p.SubCategory)
                            .ThenInclude(s => s.Category)
                            .Include(p => p.Supplier)
                            .ToList();
        }
        public List<Customer> GetCustomersPagingAndFilter(int pageIndex, int pageSize, string textSearch, string status, string dateFrom, string dateTo, string typeCustomer)
        {

            List<Customer> customers = new List<Customer>();
            //default 
            var query = from c in _context.Customers select c;

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
            return customers;
        }

        // paging bill
        public List<Bill> GetBillsPagingAndFilter(int pageIndex, int pageSize, string textSearch, string status, string dateFrom, string dateTo, string active)
        {

            List<Bill> bills = new List<Bill>();
            //default 
            var query = from b in _context.Bills select b;

            if (!string.IsNullOrEmpty(textSearch))
            {
                query = query.Where(b => b.BillId.ToString().Contains(textSearch)
                                || b.Customer.CustomerName.Contains(textSearch));
            }

            if (active != "all")
            {
                query = query.Where(b => (b.Active.ToString().Contains(active)));
            }

            if (status != "all")
            {
                query = query.Where(b => (b.BillStatus.Contains(status)));
            }

            if (!string.IsNullOrEmpty(dateFrom))
            {
                query = query.Where(b => (b.Date >= DateTime.Parse(dateFrom) && b.Date <= DateTime.Parse(dateTo)));
            }

            bills = query.Include(b => b.Customer)
                    .Include(b => b.UserNameNavigation)
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            return bills;
        }


        public InventorySheet getInventorySheetByID(int id)
        {
            return _context.InventorySheets.Where(i => i.Active == true)
                                .Include(p => p.UserNameNavigation)
                                .Where(i => i.InventorySheetId == id).FirstOrDefault();
        }
        public List<InventorySheetDetail> GetInventorySheetDetails(int inventorySheetID)
        {
            List<InventorySheetDetail> inventorySheetDetails = new List<InventorySheetDetail>();
            inventorySheetDetails = _context.InventorySheetDetails
                                            .Include(p => p.Product)
                                            .Include(p => p.InventorySheet)
                                            .Where(i => i.InventorySheetId == inventorySheetID)
                                            .ToList();
            return inventorySheetDetails;
        }
        public List<Supplier> getSupplierList()
        {
            return _context.Suppliers.Where(s => s.Active == true).ToList();
        }
        public List<ReturnOrderProduct> GetReturnOrderDetails(int returnOrderID)
        {
            return _context.ReturnOrderProducts
                                        .Include(r => r.Product)
                                        .Include(r => r.ReturnOrder)
                                        .Where(r => r.ReturnOrderId == returnOrderID)
                                        .ToList();
        }
        public ReturnOrder getReturnOrderByID(int id)
        {
            return _context.ReturnOrders.Where(i => i.Active == true)
                                .Include(p => p.UserNameNavigation)
                                .Include(r => r.Customer)
                                .Where(i => i.ReturnOrderId == id).FirstOrDefault();
        }

        // paging product
        public List<Product> GetProductsPagingAndFilter(int pageIndex, int pageSize, string textSearch, int subCategoryID, int categoryID,string location,int priceFrom, int priceTo,string inventoryLevel)
        {

            List<Product> products = new List<Product>();
            //default 
            var query = _context.Products.Where(p => p.SubCategory.CategoryId == categoryID)
                                         .Where(p => p.Active == true);
            if (subCategoryID > 0)
            {
                query = query.Where(p => p.SubCategoryId == subCategoryID);
            }
            if (!string.IsNullOrEmpty(textSearch))
            {
                query = query.Where(p => p.ProductName.ToLower().Contains(textSearch.ToLower())
                                || p.Trademark.ToLower().Contains(textSearch.ToLower())
                                || p.Supplier.Branch.ToLower().Contains(textSearch.ToLower())
                                || p.SubCategory.SubCategoryName.ToLower().Contains(textSearch.ToLower()));
            }
            //filter location
            if (location != "all")
            {
                //query = query.Where(p => p.Location == location);
            }
            //filter price range
            if (priceTo > 0)
            {
                if (priceFrom > 0)
                {
                    query = query.Where(p => p.UnitPrice >= priceFrom && p.UnitPrice <= priceTo);
                }
                else
                {
                    query = query.Where(p => p.UnitPrice <= priceTo);
                }
            }
            if (priceFrom > 0)
            {
                query = query.Where(p => p.UnitPrice >= priceFrom);
            }
            // filter inventory
            if (inventoryLevel != "all")
            {
                if (inventoryLevel == "min")
                {
                    query = query.Where(p => p.Quantity < p.InventoryLevelMin);
                }
                else
                {
                    query = query.Where(p => p.Quantity > p.InventoryLevelMax);
                }
            }
            products = query.Where(p => p.SubCategory.CategoryId == categoryID)
                    .Include(p => p.SubCategory)
                    .ThenInclude(s => s.Category)
                    .Include(p => p.Supplier)
                    .OrderBy(p => p.ProductName)
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            return products;
        }
        // paging inventory sheet
        public List<InventorySheet> getInventoryPagingAndFilter(int pageIndex, int pageSize, string textSearch, string userName, string raw_dateFrom, string raw_dateTo)
        {
            DateTime dateFrom = DateTime.Now;
            DateTime dateTo = DateTime.Now;
            //DateTime defaultDate = DateTime.Parse("0001-01-01");
            //&& (dateFrom.CompareTo(defaultDate) != 0 || dateTo.CompareTo(defaultDate) != 0)
            if (!string.IsNullOrEmpty(raw_dateFrom))
            {
                dateFrom = DateTime.Parse(raw_dateFrom);
            }
            else
            {
                raw_dateFrom = "";
            }
            if (!string.IsNullOrEmpty(raw_dateTo))
            {
                dateTo = DateTime.Parse(raw_dateTo);
            }
            else
            {
                raw_dateTo = "";
            }
            List<InventorySheet> inventories = new List<InventorySheet>();
            var query = _context.InventorySheets.Include(i => i.UserNameNavigation)
                                                .Where(i => i.Active == true)
                                                .Where(i => i.UserNameNavigation.EmployeeName.ToLower().Contains(textSearch.ToLower())
                                                            || i.Description.ToLower().Contains(textSearch.ToLower()));
            //filter date
            if (!string.IsNullOrEmpty(raw_dateFrom))
            {
                if (!string.IsNullOrEmpty(raw_dateTo))
                {
                    query = query.Where(i => i.Date >=dateFrom && i.Date <= dateTo);
                }
                else
                {
                    query = query.Where(i => i.Date >= dateFrom);
                }

            }
            if (!string.IsNullOrEmpty(raw_dateTo))
            {
                query = query.Where(i => i.Date <= dateTo);
            }

            if (userName != "all")
            {
                query = query.Where(i => i.UserName.Contains(userName));
            }
            inventories = query
                         .OrderBy(i => i.InventorySheetId)
                         .Skip((pageIndex - 1) * pageSize)
                         .Take(pageSize)
                         .ToList();
            return inventories;
        }
        //paging inventortSheetDetail
        public List<InventorySheetDetail> getInventorySheetDetailPaging(int pageIndex, int pageSize, int id)
        {
            List<InventorySheetDetail> inventorySheetDetails = new List<InventorySheetDetail>();
            inventorySheetDetails = _context.InventorySheetDetails.Include(i => i.InventorySheet)
                         .Include(i => i.Product)
                         .Include(i => i.InventorySheet.UserNameNavigation)
                         .Where(i => i.InventorySheetId == id)
                         .OrderBy(i => i.InventorySheetId)
                         .Skip((pageIndex - 1) * pageSize)
                         .Take(pageSize)
                         .ToList();
            return inventorySheetDetails;
        }
        //paging suppliers
        public List<Supplier> GetSupplierPagingAndFilter(int pageIndex, int pageSize, string textSearch)
        {
            List<Supplier> suppliers = new List<Supplier>();
            var query = _context.Suppliers.Where(s => s.Active == true);
            if (!string.IsNullOrEmpty(textSearch))
            {
                query = query.Where(s => s.Branch.ToLower().Contains(textSearch.ToLower()) || s.Address.ToLower().Contains(textSearch.ToLower()) || s.CompanyName.ToLower().Contains(textSearch.ToLower()) || s.Phone.ToLower().Contains(textSearch.ToLower())
                             || s.TaxCode.ToLower().Contains(textSearch.ToLower()));
            }
            suppliers = query.OrderBy(s => s.SupplierId)
                         .Skip((pageIndex - 1) * pageSize)
                         .Take(pageSize)
                         .ToList();
            return suppliers;
        }

        // paging return orders
        public List<ReturnOrder> getreturnOrderPagingAndFilter(int pageIndex, int pageSize, string textSearch, string userName, string status, string raw_dateFrom, string raw_dateTo)
        {
            // filter by status and date
           
            DateTime dateFrom = DateTime.Now;
            DateTime dateTo = DateTime.Now;
            if (!string.IsNullOrEmpty(raw_dateFrom))
            {
                dateFrom = DateTime.Parse(raw_dateFrom);
            }
            else
            {
                raw_dateFrom = "";
            }
            if (!string.IsNullOrEmpty(raw_dateTo))
            {
                dateTo = DateTime.Parse(raw_dateTo);
            }
            else
            {
                raw_dateTo = "";
            }

            List<ReturnOrder> returnOrders = new List<ReturnOrder>();
            var query = _context.ReturnOrders.Include(i => i.UserNameNavigation)
                                                .Include(i => i.Customer)
                                                .Where(i => i.Active == true)
                                                .Where(i => i.UserNameNavigation.EmployeeName.ToLower().Contains(textSearch.ToLower())
                                                            || i.Description.ToLower().Contains(textSearch.ToLower())
                                                            || i.Customer.CustomerName.ToLower().Contains(textSearch.ToLower()));
            if (userName != "all")
            {
                query = query.Where(i => i.UserName.Contains(userName));
            }

            if (status != "all")
            {
                Boolean _status = status == "true" ? true : false;
                query = query.Where(i => i.State == _status);
            }
            if (!string.IsNullOrEmpty(raw_dateFrom))
            {
                if (!string.IsNullOrEmpty(raw_dateTo))
                {
                    query = query.Where(i => i.Date >= dateFrom && i.Date <= dateTo);
                }
                else
                {
                    query = query.Where(i => i.Date >= dateFrom);
                }

            }
            if (!string.IsNullOrEmpty(raw_dateTo))
            {
                query = query.Where(i => i.Date <= dateTo);
            }

            returnOrders = query
                         .OrderBy(i => i.ReturnOrderId)
                         .Skip((pageIndex - 1) * pageSize)
                         .Take(pageSize)
                         .ToList();
            return returnOrders;
        }

        //paging return order
        public List<ReturnOrderProduct> getReturnOrderProductPaging(int pageIndex, int pageSize, int id)
        {
            List<ReturnOrderProduct> returnOrders = new List<ReturnOrderProduct>();
            returnOrders = _context.ReturnOrderProducts
                          .Include(r => r.ReturnOrder)
                          .Include(i => i.Product)
                          .Where(r => r.ReturnOrderId == id)
                          .OrderBy(i => i.ReturnOrderId)
                          .Skip((pageIndex - 1) * pageSize)
                          .Take(pageSize)
                          .ToList();
            return returnOrders;
        }

        //get billDetails by day
        public List<BillDetail> GetBillDetails(DateTime date)
        {
            return _context.BillDetails
                                .Include(b => b.Bill)
                                .Include(b => b.Product)
                                .Where(b => b.Bill.Date == date).ToList();
        }
        //get GetReturOrder by day
        public List<ReturnOrder> GetReturOrderByDay(DateTime date)
        {
            return _context.ReturnOrders
                                .Where(b => b.Date == date).ToList();
        }

        internal IList<Oder> GetOrdersPagingAndFilter(int pageIndex, int pageSize, string textSearch, string status, string dateFrom, string estDateFrom, string estDateTo, string dateTo, string active)
        {
            List<Oder> orders = new List<Oder>();
            //default 
            var query = from o in _context.Oders select o;

            if (!string.IsNullOrEmpty(textSearch))
            {
                query = query.Where(o => (o.OderId.ToString().Contains(textSearch)
                                 || o.Shipper.ShipperName.Contains(textSearch)
                                 || o.Customer.CustomerName.Contains(textSearch)));
            }

            if (active != "all")
            {
                query = query.Where(o => (o.Active.ToString().Contains(active)));
            }

            if (status != "all")
            {
                query = query.Where(o => (o.OderState.Contains(status)));
            }

            if (!string.IsNullOrEmpty(dateFrom))
            {
                query = query.Where(o => (o.OrderDate >= DateTime.Parse(dateFrom) && o.OrderDate <= DateTime.Parse(dateTo)));
            }

            if (!string.IsNullOrEmpty(estDateTo))
            {
                query = query.Where(o => (o.EstimatedDate >= DateTime.Parse(estDateFrom) && o.EstimatedDate <= DateTime.Parse(estDateTo)));
            }

            orders = query.Include(o => o.Customer)
                    .Include(o => o.Shipper)
                    .Include(b => b.UserNameNavigation)
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            return orders;
        }
    }
}
