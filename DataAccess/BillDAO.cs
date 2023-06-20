using BusinessObjects.WahoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class BillDAO
    {
        public static List<BillDetail> GetBillDetailById(int billId)
        {
            List<BillDetail> billDetails = new List<BillDetail>();
            try
            {
                using (var _context = new WahoS8Context())
                {
                    billDetails = _context.BillDetails.Where(b => b.BillId == billId).ToList();
                    return billDetails;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static BillDetail GetBillDetailByIdAndProID(int billId, int productId)
        {
            BillDetail billDetail = new BillDetail();
            try
            {
                using (var _context = new WahoS8Context())
                {
                    billDetail = _context.BillDetails.FirstOrDefault(b=> b.BillId == billId && b.ProductId == productId);
                    return billDetail;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
