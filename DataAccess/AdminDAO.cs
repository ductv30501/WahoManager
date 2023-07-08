using BusinessObjects.WahoModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.DashBoardViewModels;

namespace DataAccess
{
    public class AdminDAO
    {
        public static int TotalBillInDay()
        {
            try
            {
                using (var _context = new WahoS8Context())
                {
                    return _context.Bills.Where(b => b.Date == DateTime.Today).Count();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static List<BillDetail> BillDetails(DateTime date)
        {
            List<BillDetail> billDetails = new List<BillDetail>(); 
            try
            {
                using (var _context = new WahoS8Context())
                {
                    billDetails = _context.BillDetails.Include(b => b.Bill)
                                                     .Include(b => b.Product)
                                                     .Where(b => b.Bill.Date == date).ToList();
                    return billDetails;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static int TotalReturnInDay()
        {
            try
            {
                using (var _context = new WahoS8Context())
                {
                    return _context.ReturnOrders.Where(b => b.Date == DateTime.Today).Count();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static List<ReturnOrder> ReturnOrdersInDay()
        {
            List<ReturnOrder> list = new List<ReturnOrder>();
            try
            {
                using (var _context = new WahoS8Context())
                {
                    list = _context.ReturnOrders.Where(b => b.Date == DateTime.Today).ToList();
                    return list;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        // theo doanh số bill trong tháng
        public static List<TotalMMVM> totalBillMMVMs(int month, int year) {
            List<TotalMMVM> list = new List<TotalMMVM>();
            try
            {
                using (var _context = new WahoS8Context())
                {
                    var query = from bd in _context.BillDetails
                                join b in _context.Bills on bd.BillId equals b.BillId
                                join p in _context.Products on bd.ProductId equals p.ProductId
                                where b.Date.Month == month && b.Date.Year == year
                                group new { p.ProductName, bd.Quantity, p.UnitPrice, bd.Discount } by p.ProductName into g
                                orderby g.Sum(x => x.Quantity * x.UnitPrice * (1 - x.Discount)) descending
                                select new TotalMMVM{ ProductName = g.Key, TotalQuantity = g.Sum(x => x.Quantity * x.UnitPrice * (1 - x.Discount)) } ;
                    list = query.Take(10).ToList();
                    return list;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        // theo doanh số order trong tháng
        public static List<TotalMMVM> totalOrdersMMVMs(int month, int year)
        {
            List<TotalMMVM> list = new List<TotalMMVM>();
            try
            {
                using (var _context = new WahoS8Context())
                {
                    var queryOrder = from bd in _context.OderDetails
                                     join b in _context.Oders on bd.OderId equals b.OderId
                                     join p in _context.Products on bd.ProductId equals p.ProductId
                                     where b.OrderDate.Month == month && b.OrderDate.Year == year
                                     group new { p.ProductName, bd.Quantity, p.UnitPrice, bd.Discount } by p.ProductName into g
                                     orderby g.Sum(x => x.Quantity * x.UnitPrice * (1 - x.Discount)) descending
                                     select new TotalMMVM { ProductName = g.Key, TotalQuantity = g.Sum(x => x.Quantity * x.UnitPrice * (1 - x.Discount)) };
                    list = queryOrder.Take(10).ToList();
                    return list;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        // theo só lượng của bill trong tháng
        public static List<TotalMMVM> totalNumberBillMs(int month, int year)
        {
            List<TotalMMVM> list = new List<TotalMMVM>();
            try
            {
                using (var _context = new WahoS8Context())
                {
                    var query = from bd in _context.BillDetails
                                join b in _context.Bills on bd.BillId equals b.BillId
                                join p in _context.Products on bd.ProductId equals p.ProductId
                                where b.Date.Month == month && b.Date.Year == year
                                group new { p.ProductName, bd.Quantity } by p.ProductName into g
                                orderby g.Sum(x => x.Quantity) descending
                                select new TotalMMVM { ProductName = g.Key, TotalQuantity = g.Sum(x => x.Quantity) };
                    list = query.Take(10).ToList();
                    return list;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        // theo só lượng của order trong tháng
        public static List<TotalMMVM> totalNummberOrdersMMVMs(int month, int year)
        {
            List<TotalMMVM> list = new List<TotalMMVM>();
            try
            {
                using (var _context = new WahoS8Context())
                {
                    var queryOrder = from bd in _context.OderDetails
                                     join b in _context.Oders on bd.OderId equals b.OderId
                                     join p in _context.Products on bd.ProductId equals p.ProductId
                                     where b.OrderDate.Month == month && b.OrderDate.Year == year
                                     group new { p.ProductName, bd.Quantity } by p.ProductName into g
                                     orderby g.Sum(x => x.Quantity) descending
                                     select new TotalMMVM { ProductName = g.Key, TotalQuantity = g.Sum(x => x.Quantity) };
                    list = queryOrder.Take(10).ToList();
                    return list;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        // doanh số theo ngày trong tháng trong tháng
        //theo doanh số bill
        public static List<DayInMonth> totalNumberBillDayInMs(int month, int year)
        {
            List<DayInMonth> list = new List<DayInMonth>();
            try
            {
                using (var _context = new WahoS8Context())
                {
                    var queryDay = from bd in _context.BillDetails
                                   join b in _context.Bills on bd.BillId equals b.BillId
                                   join p in _context.Products on bd.ProductId equals p.ProductId
                                   where b.Date.Month == month && b.Date.Year == year
                                   group new { bd.Quantity, p.UnitPrice, bd.Discount } by b.Date.Day into g
                                   orderby g.Key ascending
                                   select new DayInMonth { Day = g.Key, TotalQuantity = g.Sum(x => x.Quantity * x.UnitPrice * (1 - x.Discount)) };
                    list = queryDay.Take(10).ToList();
                    return list;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        // order
        public static List<DayInMonth> totalNumberOrdersDayInMs(int month, int year)
        {
            List<DayInMonth> list = new List<DayInMonth>();
            try
            {
                using (var _context = new WahoS8Context())
                {
                    var queryOrderDay = from bd in _context.OderDetails
                                        join b in _context.Oders on bd.OderId equals b.OderId
                                        join p in _context.Products on bd.ProductId equals p.ProductId
                                        where b.OrderDate.Month == month && b.OrderDate.Year == year
                                        group new { bd.Quantity, p.UnitPrice, bd.Discount } by b.OrderDate.Day into g
                                        orderby g.Key ascending
                                        select new DayInMonth { Day = g.Key, TotalQuantity = g.Sum(x => x.Quantity * x.UnitPrice * (1 - x.Discount)) };
                    list = queryOrderDay.Take(10).ToList();
                    return list;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
