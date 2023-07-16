using BusinessObjects.WahoModels;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.BillDetailViewModels;
using ViewModels.BillViewModel;

namespace Repositories.BillRepository
{
    public class BillRepositories : IBillRepositories
    {
        public void AddListBillDetail(List<BillDetailVM> billDetailsVM)
        {
            BillDAO.AddListBillDetail(billDetailsVM);
        }

        public int CountPagingBill(int pageIndex, int pageSize, string textSearch, string status, string dateFrom, string dateTo, string active, int wahoId)
        {
            return BillDAO.CountPagingBill(pageIndex, pageSize, textSearch, status, dateFrom, dateTo, active, wahoId);
        }

        public List<BillDetail> GetBillDetailById(int billId) => BillDAO.GetBillDetailById(billId);

        public BillDetail GetBillDetailByIdAndProID(int billId, int productId) => BillDAO.GetBillDetailByIdAndProID(billId,productId);

        public List<Bill> GetBillsPagingAndFilter(int pageIndex, int pageSize, string textSearch, string status, string dateFrom, string dateTo, string active, int wahoId)
        {
            return BillDAO.GetBillsPagingAndFilter(pageIndex, pageSize, textSearch, status, dateFrom, dateTo, active, wahoId);
        }

        public int saveBill(PostBill PostBill)
        {
            return BillDAO.saveBill(PostBill);
        }

        public void UpdateBill(PostBill postBill)
        {
            BillDAO.UpdateBill(postBill);
        }
    }
}
