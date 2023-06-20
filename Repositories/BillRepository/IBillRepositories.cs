using BusinessObjects.WahoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.BillRepository
{
    public interface IBillRepositories
    {
        List<BillDetail> GetBillDetailById(int billId);
        BillDetail GetBillDetailByIdAndProID(int billId, int productId);
    }
}
