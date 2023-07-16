using AutoMapper;
using BusinessObjects.WahoModels;
using DataAccess.AutoMapperConfig;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.BillDetailViewModels;
using ViewModels.BillViewModel;
using ViewModels.OrderDetailViewModels;
using ViewModels.OrderViewModels;

namespace DataAccess
{
    public class BillDAO
    {
        private static readonly IMapper _mapper = BillMapper.ConfigureVMtoM();
        private static readonly IMapper _mapperDetail = BillDetailMapper.ConfigureVMtoM();
        public static void AddListBillDetail(List<BillDetailVM> billDetailsVM)
        {
            List<BillDetail> billDetails = new List<BillDetail>();
            foreach (var o in billDetailsVM)
            {
                BillDetail billDetail = _mapperDetail.Map<BillDetail>(o);
                billDetails.Add(billDetail);
            }
            try
            {
                using (var _context = new WahoS8Context())
                {
                    _context.AddRange(billDetails);
                    _context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static int CountPagingBill(int pageIndex, int pageSize, string textSearch, string status, string dateFrom, string dateTo, string active, int wahoId)
        {
            try
            {
                using (var context = new WahoS8Context())
                {
                    var query = context.Bills.Where(c => c.WahoId == wahoId).AsQueryable();
                    else
                    {
                        textSearch = "";
                    }
                    if (!string.IsNullOrEmpty(textSearch))
                    {
                        query = query.Where(b => (b.BillId.ToString().Contains(textSearch)
                                 || b.Customer.CustomerName.Contains(textSearch)));
                    }
                    if (active != "all")
                    {
                        query = query.Where(b => (b.Active.ToString().Contains(active)));
                    }

                    if (status != "all")
                    {
                        query = query.Where(b => (b.BillStatus.Contains(status)));
                    }

                    if (!string.IsNullOrEmpty(dateFrom))
                    {
                        query = query.Where(b => (b.Date >= DateTime.Parse(dateFrom) && b.Date <= DateTime.Parse(dateTo)));
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
                    if (productId != null || productId > 0)
                    {
                        billDetail = _context.BillDetails.FirstOrDefault(b => b.BillId == billId && b.ProductId == productId);
                    }
                    else
                    {
                        billDetail = _context.BillDetails.FirstOrDefault(b => b.BillId == billId);

                    }
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
            try
            {
                List<Bill> bills = new List<Bill>();
                //default 
                using (var context = new WahoS8Context())
                {
                    var query = from b in context.Bills select b;

                    if (!string.IsNullOrEmpty(textSearch))
                    {
                        query = query.Where(b => b.BillId.ToString().Contains(textSearch)
                                        || b.Customer.CustomerName.Contains(textSearch));
                    }

                    if (active != "all")
                    {
                        query = query.Where(b => (b.Active.ToString().Contains(active)));
                    }

                    if (status != "all")
                    {
                        query = query.Where(b => (b.BillStatus.Contains(status)));
                    }

                    if (!string.IsNullOrEmpty(dateFrom))
                    {
                        query = query.Where(b => (b.Date >= DateTime.Parse(dateFrom) && b.Date <= DateTime.Parse(dateTo)));
                    }

                    bills = query.Include(b => b.Customer)
                            .Include(b => b.UserNameNavigation)
                            .Skip((pageIndex - 1) * pageSize)
                            .Take(pageSize)
                            .ToList();
                    return bills;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static int saveBill(PostBill postBill)
        {
            Bill bill = _mapper.Map<Bill>(postBill);
            try
            {
                using (var _context = new WahoS8Context())
                {
                    _context.Bills.Add(bill);
                    _context.SaveChanges();
                    return bill.BillId;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void UpdateBill(PostBill postBill)
        {
            Bill bill = _mapper.Map<Bill>(postBill);
            try
            {
                using (var _context = new WahoS8Context())
                {
                    _context.Entry<Bill>(bill).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
