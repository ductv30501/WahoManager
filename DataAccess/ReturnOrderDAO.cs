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
using ViewModels.ReturnOrderViewModels;

namespace DataAccess
{
    public class ReturnOrderDAO
    {
        private static readonly IMapper _mapper = ReturnOrderMapper.ConfigureVMtoM();
        private static readonly IMapper _mapperDetail = ReturnOrderProductMapper.ConfigureVMtoM();
        public static List<ReturnOrder> returnOrdersByBillId(int billId, int wahoId)
        {
            List<ReturnOrder> returnOrders = new List<ReturnOrder>();
            try
            {
                using (var _context = new WahoS8Context())
                {
                    returnOrders = _context.ReturnOrders.Where(r => r.BillId == billId && r.WahoId== wahoId).ToList();
                    return returnOrders;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static List<ReturnOrderProduct> ReturnOrderProductsByReturnID(int returnId)
        {
            List<ReturnOrderProduct> returnOrderProduct = new List<ReturnOrderProduct>();
            try
            {
                using (var _context = new WahoS8Context())
                {
                    returnOrderProduct = _context.ReturnOrderProducts.Include(p => p.Product).Where(r => r.ReturnOrderId == returnId).ToList();
                    return returnOrderProduct;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static int saveReturnOrder(ReturnOrderVM returnOrderVM)
        {
            ReturnOrder returnOrder = _mapper.Map<ReturnOrder>(returnOrderVM);
            try
            {
                using (var _context = new WahoS8Context())
                {
                    _context.ReturnOrders.Add(returnOrder);
                    _context.SaveChanges();
                    return returnOrder.ReturnOrderId;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static void saveListReturnOrderProduct(List<ReturnOrderProductVM> returnOrderProductsVM)
        {
            List<ReturnOrderProduct> List = new List<ReturnOrderProduct>();
            foreach (var rvm in returnOrderProductsVM)
            {
                ReturnOrderProduct returnOrderProduct = _mapperDetail.Map<ReturnOrderProduct>(rvm);
                List.Add(returnOrderProduct);
            }
            try
            {
                using (var _context = new WahoS8Context())
                {
                    _context.ReturnOrderProducts.AddRange(List);
                    _context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static List<ReturnOrder> returnOrdersPaging(int pageIndex, int pageSize, string textSearch, string userName, string status, string raw_dateFrom, string raw_dateTo, int wahoId)
        {
            List<ReturnOrder> returnOrders = new List<ReturnOrder>();
            try
            {
                using (var _context = new WahoS8Context())
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

                    var query = _context.ReturnOrders.Include(i => i.UserNameNavigation)
                                                        .Include(i => i.Customer)
                                                        .Where(i => i.Active == true && i.WahoId == wahoId).AsQueryable();
                    if (!string.IsNullOrEmpty(textSearch))
                    {
                        query = query.Where(i => i.UserNameNavigation.EmployeeName.ToLower().Contains(textSearch.ToLower())
                                                                    || i.Description.ToLower().Contains(textSearch.ToLower())
                                                                    || i.Customer.CustomerName.ToLower().Contains(textSearch.ToLower()));
                    }
                                                        
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
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static int countReturnOrder(string textSearch, string employeeID, string status, string dateFrom, string dateTo, int wahoId)
        {
            int count = 0;
            try
            {
                using (var _context = new WahoS8Context())
                {
                    var query = _context.ReturnOrders.Include(p => p.UserNameNavigation)
                           .Include(i => i.Customer)
                           .Where(i => i.Active == true && i.WahoId == wahoId).AsQueryable();
                    if (!string.IsNullOrEmpty(textSearch))
                    {
                        query = query.Where(i => i.UserNameNavigation.EmployeeName.ToLower().Contains(textSearch.ToLower())
                                   || i.Description.ToLower().Contains(textSearch.ToLower())
                                   || i.Customer.CustomerName.ToLower().Contains(textSearch.ToLower()));
                    }
                    if (employeeID != "all")
                    {
                        query = query.Where(i => i.UserName == employeeID);
                    }
                    // check status to filter
                    if (status != "all")
                    {
                        Boolean _status = status == "true" ? true : false;
                        query = query.Where(i => i.State == _status);
                    }
                    // compare date to filter
                    if (!string.IsNullOrEmpty(dateFrom))
                    {
                        if (!string.IsNullOrEmpty(dateTo))
                        {
                            query = query.Where(i => i.Date >= DateTime.Parse(dateFrom) && i.Date <= DateTime.Parse(dateTo));
                        }
                        else
                        {
                            query = query.Where(i => i.Date >= DateTime.Parse(dateFrom));
                        }
                    }
                    if (!string.IsNullOrEmpty(dateTo))
                    {
                        query = query.Where(i => i.Date <= DateTime.Parse(dateTo));
                    }
                    count = query.Count();
                    return count;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
