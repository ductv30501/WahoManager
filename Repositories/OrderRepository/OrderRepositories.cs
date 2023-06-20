using BusinessObjects.WahoModels;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.OrderDetailViewModels;
using ViewModels.OrderViewModels;

namespace Repositories.OrderRepository
{
    public class OrderRepositories : IOrderRepositories
    {
        public void AddListOrderDetail(List<OrderDetailVM> orderDetailVMs) => OrderDAO.AddListOrderDetail(orderDetailVMs);

        public List<Shipper> GetListShipperSearch(string textSearch, int wahoId) => OrderDAO.GetListShipperSearch(textSearch, wahoId);

        public Oder GetOrderById(int orderId) => OrderDAO.GetOrderById(orderId); 

        public int GetOrderCount(string textSearch, string active, string status, string dateTo, string dateFrom, string estDateTo, string estDateFrom, int wahoId) 
            => OrderDAO.GetOrderCount(textSearch, active, status, dateTo, dateFrom, estDateTo, estDateFrom, wahoId);

        public List<OderDetail> GetOrderDetailById(int orderId) => OrderDAO.GetOrderDetailById(orderId);

        public OderDetail GetOrderDetailByIdAndProId(int orderId, int productId) => OrderDAO.GetOrderDetailByIdAndProId(orderId, productId);

        public List<Oder> GetOrdersPaging(int pageIndex, int pageSize, string textSearch, string status, string dateFrom, string estDateFrom, string estDateTo, string dateTo, string active, int wahoId)
            => OrderDAO.GetOrdersPaging(pageIndex, pageSize, textSearch, status, dateFrom, estDateFrom, estDateTo, dateTo, active, wahoId);

        public int SaveOrder(OrderVM orderVM) => OrderDAO.SaveOrder(orderVM);

        public void UpdateOrder(OrderVM orderVM) => OrderDAO.UpdateOrder(orderVM);
    }
}
