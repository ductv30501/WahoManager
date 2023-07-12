using BusinessObjects.WahoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ReturnOrderViewModels;

namespace Repositories.ReturnOrderRepository
{
    public interface IReturnOrderRepositories
    {
        List<ReturnOrder> returnOrdersPaging(int pageIndex, int pageSize, string textSearch, string userName, string status, string raw_dateFrom, string raw_dateTo, int wahoId);
        int countReturnOrder(string textSearch, string employeeID, string status, string dateFrom, string dateTo, int wahoId);
        List<ReturnOrder> returnOrdersByBillId(int billId, int wahoId);
        List<ReturnOrderProduct> ReturnOrderProductsByReturnID(int returnId);
        int saveReturnOrder(ReturnOrderVM returnOrderVM);
        void saveListReturnOrderProduct(List<ReturnOrderProductVM> returnOrderProductsVM);
        ReturnOrder GetReturnOrderByID(int returnOrderId);
        List<ReturnOrderProduct> RTOProductsPaging(int pageIndex, int pageSize, int id);
        void UpdateReturnOrder(ReturnOrderVM returnOrderVM);
    }
}
