using BusinessObjects.WahoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.BillViewModel;

namespace Repositories.BillRepository
{
    public interface IBillRepositories
    {
        public int CountPagingBill(int pageIndex, int pageSize, string textSearch, string status, string dateFrom, string dateTo, string active, int wahoId);
        List<BillDetail> GetBillDetailById(int billId);
        BillDetail GetBillDetailByIdAndProID(int billId, int productId);
        public List<Bill> GetBillsPagingAndFilter(int pageIndex, int pageSize, string textSearch, string status, string dateFrom, string dateTo, string active, int wahoId);
    }
}
