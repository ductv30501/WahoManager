using BusinessObjects.WahoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.OrderDetailViewModels;
using ViewModels.OrderViewModels;

namespace Repositories.OrderRepository
{
    public interface IOrderRepositories
    {
        int GetOrderCount(string textSearch, string active, string status, string dateTo, string dateFrom, string estDateTo, string estDateFrom, int wahoId);

        List<Oder> GetOrdersPaging(int pageIndex, int pageSize, string textSearch, string status, string dateFrom, string estDateFrom, string estDateTo, string dateTo, string active, int wahoId);
        List<Shipper> GetListShipperSearch(string textSearch, int wahoId);
        int SaveOrder(OrderVM orderVM);
        void AddListOrderDetail(List<OrderDetailVM> orderDetailVMs);
        Oder GetOrderById(int orderId);
        List<OderDetail> GetOrderDetailById(int orderId);
        void UpdateOrder(OrderVM orderVM);

    }
}
