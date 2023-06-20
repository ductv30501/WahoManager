using AutoMapper;
using BusinessObjects.WahoModels;
using DataAccess.AutoMapperConfig;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Noding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using ViewModels.OrderDetailViewModels;
using ViewModels.OrderViewModels;

namespace DataAccess
{
    public class OrderDAO
    {
        private static readonly IMapper _mapper = OrderMapper.ConfigureVMtoM();
        private static readonly IMapper _mapperDetail = OrderDetailMapper.ConfigureVMtoM();
        public static void AddListOrderDetail(List<OrderDetailVM> orderDetailVMs)
        {
            List<OderDetail> orderDetails = new List<OderDetail>();
            foreach (var o in orderDetailVMs)
            {
                OderDetail oderDetail = _mapperDetail.Map<OderDetail>(o);
                orderDetails.Add(oderDetail);
            }
            try
            {
                using (var _context = new WahoS8Context())
                {
                    _context.AddRange(orderDetails);
                    _context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static Oder GetOrderById(int orderId)
        {
            Oder order = new Oder();
            try
            {
                using (var _context = new WahoS8Context())
                {
                    order = _context.Oders.Include(b => b.UserNameNavigation)
                                            .Include(b => b.Customer)
                                            .Include(b => b.Shipper)
                                            .FirstOrDefault(o => o.OderId == orderId);
                    return order;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static void UpdateOrder(OrderVM orderVM)
        {
            Oder order = _mapper.Map<Oder>(orderVM);
            try
            {
                using (var _context = new WahoS8Context())
                {
                    _context.Entry<Oder>(order).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static List<OderDetail> GetOrderDetailById(int orderId)
        {
            List<OderDetail> orderDetails = new List<OderDetail>(); 
            try
            {
                using (var _context = new WahoS8Context())
                {
                    orderDetails = _context.OderDetails.Include(bd => bd.Product).Where(o => o.OderId == orderId).ToList();
                    return orderDetails;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static OderDetail GetOrderDetailByIdAndProId(int orderId, int productId)
        {
            OderDetail orderDetail = new OderDetail();
            try
            {
                using (var _context = new WahoS8Context())
                {
                    orderDetail = _context.OderDetails.Include(bd => bd.Product).FirstOrDefault(o => o.OderId == orderId && o.ProductId == productId);
                    return orderDetail;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static int SaveOrder(OrderVM orderVM)
        {
            Oder order = _mapper.Map<Oder>(orderVM);
            try
            {
                using (var _context = new WahoS8Context())
                {
                    _context.Oders.Add(order);
                    _context.SaveChanges();
                    return order.OderId;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static List<Shipper> GetListShipperSearch(string textSearch, int wahoId)
        {
            List<Shipper> shippers = new List<Shipper>();
            try
            {
                using (var _context = new WahoS8Context())
                {
                    var query = _context.Shippers.AsQueryable();
                    if (!string.IsNullOrEmpty(textSearch))
                    {
                        query = query.Where(s => s.ShipperName.ToLower().Contains(textSearch.ToLower()) || s.Phone.Contains(textSearch));
                    }
                    shippers = query.Where(s => s.WahoId == wahoId).ToList();
                    return shippers;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static List<Oder> GetOrdersPaging(int pageIndex, int pageSize, string textSearch, string status, string dateFrom, string estDateFrom, string estDateTo, string dateTo, string active, int wahoId)
        {
            List <Oder> orders = new List<Oder>();
            try
            {
                using (var _context = new WahoS8Context())
                {
                    var query = _context.Oders.AsQueryable();

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
                        if (!string.IsNullOrEmpty(dateTo))
                        {
                            query = query.Where(o => o.OrderDate >= DateTime.Parse(dateFrom) && o.OrderDate <= DateTime.Parse(dateTo));
                        }
                        else
                        {
                            query = query.Where(o => o.OrderDate >= DateTime.Parse(dateFrom));
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(dateTo))
                        {
                            query = query.Where(o => o.OrderDate <= DateTime.Parse(dateTo));
                        }
                    }

                    if (!string.IsNullOrEmpty(estDateFrom))
                    {
                        if (!string.IsNullOrEmpty(estDateTo))
                        {
                            query = query.Where(o => o.EstimatedDate >= DateTime.Parse(estDateFrom) && o.EstimatedDate <= DateTime.Parse(estDateTo));
                        }
                        else
                        {
                            query = query.Where(o => o.EstimatedDate >= DateTime.Parse(estDateFrom));
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(estDateTo))
                        {
                            query = query.Where(o => o.EstimatedDate <= DateTime.Parse(estDateTo));
                        }
                    }
                    //if (!string.IsNullOrEmpty(dateFrom))
                    //{
                    //    query = query.Where(o => (o.OrderDate >= DateTime.Parse(dateFrom) && o.OrderDate <= DateTime.Parse(dateTo)));
                    //}

                    //if (!string.IsNullOrEmpty(estDateTo))
                    //{
                    //    query = query.Where(o => (o.EstimatedDate >= DateTime.Parse(estDateFrom) && o.EstimatedDate <= DateTime.Parse(estDateTo)));
                    //}

                    orders = query.Include(o => o.Customer)
                            .Include(o => o.Shipper)
                            .Include(b => b.UserNameNavigation)
                            .Where(o => o.WahoId == wahoId)
                            .Skip((pageIndex - 1) * pageSize)
                            .Take(pageSize)
                            .ToList();
                    return orders;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        public static int GetOrderCount(string textSearch, string active, string status,string dateTo, string dateFrom, string estDateTo, string estDateFrom, int wahoId)
        {
            int count = 0;
            try
            {
                using (var _context = new WahoS8Context())
                {
                    var raw_filterForTotalCount = _context.Oders
                            .Include(o => o.Customer)
                            .Include(o => o.Shipper)
                            .Where(o => o.WahoId == wahoId).AsQueryable();
                    if (!string.IsNullOrEmpty(textSearch)) {
                        raw_filterForTotalCount = raw_filterForTotalCount.Where(o => o.OderId.ToString().Contains(textSearch)
                                || o.Shipper.ShipperName.Contains(textSearch)
                                || o.Customer.CustomerName.Contains(textSearch));
                    }
                    if (active != "all")
                    {
                        raw_filterForTotalCount = raw_filterForTotalCount.Where(o => o.Active.ToString().Contains(active));
                    }

                    if (status != "all")
                    {
                        raw_filterForTotalCount = raw_filterForTotalCount.Where(o => (o.OderState.Contains(status)));
                    }

                    if (!string.IsNullOrEmpty(dateFrom))
                    {
                        if (!string.IsNullOrEmpty(dateTo))
                        {
                            raw_filterForTotalCount = raw_filterForTotalCount.Where(o => o.OrderDate >= DateTime.Parse(dateFrom) && o.OrderDate <= DateTime.Parse(dateTo));
                        }
                        else
                        {
                            raw_filterForTotalCount = raw_filterForTotalCount.Where(o => o.OrderDate >= DateTime.Parse(dateFrom));
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(dateTo))
                        {
                            raw_filterForTotalCount = raw_filterForTotalCount.Where(o => o.OrderDate <= DateTime.Parse(dateTo));
                        }
                    }

                    if (!string.IsNullOrEmpty(estDateFrom))
                    {
                        if (!string.IsNullOrEmpty(estDateTo))
                        {
                            raw_filterForTotalCount = raw_filterForTotalCount.Where(o => o.EstimatedDate >= DateTime.Parse(estDateFrom) && o.EstimatedDate <= DateTime.Parse(estDateTo));
                        }
                        else
                        {
                            raw_filterForTotalCount = raw_filterForTotalCount.Where(o => o.EstimatedDate >= DateTime.Parse(estDateFrom));
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(estDateTo))
                        {
                            raw_filterForTotalCount = raw_filterForTotalCount.Where(o => o.EstimatedDate <= DateTime.Parse(estDateTo));
                        }
                    }

                    //if (!string.IsNullOrEmpty(estDateTo))
                    //{
                    //    raw_filterForTotalCount = raw_filterForTotalCount.Where(o => (o.EstimatedDate >= DateTime.Parse(estDateFrom) && o.EstimatedDate <= DateTime.Parse(estDateTo)));
                    //}
                    // count order list
                    count = raw_filterForTotalCount.Count();
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
