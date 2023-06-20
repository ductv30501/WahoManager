using BusinessObjects.WahoModels;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.BillRepository
{
    public class BillRepositories : IBillRepositories
    {
        public List<BillDetail> GetBillDetailById(int billId) => BillDAO.GetBillDetailById(billId);

        public BillDetail GetBillDetailByIdAndProID(int billId, int productId) => BillDAO.GetBillDetailByIdAndProID(billId,productId);
    }
}
