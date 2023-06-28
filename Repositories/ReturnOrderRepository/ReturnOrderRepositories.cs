using BusinessObjects.WahoModels;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.EmployeeViewModels;
using ViewModels.ReturnOrderViewModels;

namespace Repositories.ReturnOrderRepository
{
    public class ReturnOrderRepositories : IReturnOrderRepositories
    {
        public int countReturnOrder(string textSearch, string employeeID, string status, string dateFrom, string dateTo, int wahoId) => ReturnOrderDAO.countReturnOrder(textSearch, employeeID, status, dateFrom, dateTo, wahoId);

        public ReturnOrder GetReturnOrderByID(int returnOrderId) => ReturnOrderDAO.GetReturnOrderByID(returnOrderId);

        public List<ReturnOrderProduct> ReturnOrderProductsByReturnID(int returnId) => ReturnOrderDAO.ReturnOrderProductsByReturnID(returnId);

        public List<ReturnOrder> returnOrdersByBillId(int billId, int wahoId) => ReturnOrderDAO.returnOrdersByBillId(billId, wahoId);

        public List<ReturnOrder> returnOrdersPaging(int pageIndex, int pageSize, string textSearch, string userName, string status, string raw_dateFrom, string raw_dateTo, int wahoId)
            => ReturnOrderDAO.returnOrdersPaging(pageIndex, pageSize, textSearch, userName, status, raw_dateFrom, raw_dateTo,wahoId);

        public List<ReturnOrderProduct> RTOProductsPaging(int pageIndex, int pageSize, int id) => ReturnOrderDAO.RTOProductsPaging(pageIndex, pageSize, id);

        public void saveListReturnOrderProduct(List<ReturnOrderProductVM> returnOrderProductsVM) => ReturnOrderDAO.saveListReturnOrderProduct(returnOrderProductsVM);

        public int saveReturnOrder(ReturnOrderVM returnOrderVM) => ReturnOrderDAO.saveReturnOrder(returnOrderVM);

        public void UpdateReturnOrder(ReturnOrderVM returnOrderVM) => ReturnOrderDAO.UpdateReturnOrder(returnOrderVM);
    }
}
