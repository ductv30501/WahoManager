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
        public static int CountPagingBill(int pageIndex, int pageSize, string textSearch, string status, string dateFrom, string dateTo, string active, int wahoId)
        {
            try
            {
                using (var context = new WahoS8Context())
                {
                    var query = context.Bills.Where(c => c.WahoId == wahoId).AsQueryable();
                    if (!string.IsNullOrEmpty(textSearch))
                    {
                        query = query.Where(c => (c.CustomerName.Contains(textSearch)
                                         || c.Phone.Contains(textSearch)
                                         || c.Email.Contains(textSearch)
                                         || c.TaxCode.Contains(textSearch)));
                    }
                    if (status != "all")
                    {
                        query = query.Where(c => c.Active.ToString().Contains(status));
                    }
                    if (typeCustomer != "all")
                    {
                        query = query.Where(c => (c.TypeOfCustomer.ToString().Contains(typeCustomer)));
                    }
                    if (!string.IsNullOrEmpty(dateFrom))
                    {
                        query = query.Where(c => (c.Dob >= DateTime.Parse(dateFrom) && c.Dob <= DateTime.Parse(dateTo)));
                    }

                    return query.Count();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

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

        public static List<Bill> GetBillsPagingAndFilter(int pageIndex, int pageSize, string textSearch, string status, string dateFrom, string dateTo, string active, int wahoId)
        {
            throw new NotImplementedException();
        }
    }
}
