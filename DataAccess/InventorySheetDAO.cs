using AutoMapper;
using BusinessObjects.WahoModels;
using DataAccess.AutoMapperConfig;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.EmployeeViewModels;
using ViewModels.InventorySheetViewModels;
using ViewModels.ProductViewModels;

namespace DataAccess
{
    public class InventorySheetDAO
    {
        private static readonly IMapper _mapper = InventorySheetMapper.ConfigureVMtoM();
        private static readonly IMapper _mapperDetail = InventorySheetDetailMapper.ConfigureVMtoM();
        public static List<InventorySheet> GetInventorySheets(int wahoId)
        {
            try
            {
                using (var _context = new WahoS8Context())
                {
                    return _context.InventorySheets
                        .Where(sb => sb.WahoId == wahoId)
                        .ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        public static int SaveInventory(InventorySheetVM inventoryVM)
        {
            InventorySheet inventory = _mapper.Map<InventorySheet>(inventoryVM);
            try
            {
                using (var _context = new WahoS8Context())
                {
                    _context.InventorySheets.Add(inventory);
                    _context.SaveChanges();
                    return inventory.InventorySheetId;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static void saveInventoryDetail(List<InventorySheetDetailVM> InDeVm)
        {
            //InventorySheet inventory = _mapper.Map<InventorySheet>(inventoryVM);
            List<InventorySheetDetail> inDetails = new List<InventorySheetDetail>();
            foreach (var IDVM in InDeVm)
            {
                InventorySheetDetail inDe = new InventorySheetDetail();
                inDe = _mapperDetail.Map<InventorySheetDetail>(IDVM);
                inDetails.Add(inDe);
            }
            try
            {
                using (var _context = new WahoS8Context())
                {
                    _context.InventorySheetDetails.AddRange(inDetails);
                    _context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static void UpdateInventorySheet(InventorySheetVM inventoryVM)
        {
            InventorySheet inventory = _mapper.Map<InventorySheet>(inventoryVM);
            try
            {
                using (var _context = new WahoS8Context())
                {
                    _context.Entry<InventorySheet>(inventory).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        public static InventorySheetDetail GetInventorySheetDetailByIDAndProId(int pro_id,int inventoryid)
        {
            try
            {
                using (var _context = new WahoS8Context())
                {
                    return _context.InventorySheetDetails
                        .FirstOrDefault(sb => sb.InventorySheetId == inventoryid && sb.ProductId == pro_id);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static void UpdateInventorySheetDetail(InventorySheetDetailVM inventoryDetailVM)
        {
            InventorySheetDetail inventoryDe = _mapperDetail.Map<InventorySheetDetail>(inventoryDetailVM);
            try
            {
                using (var _context = new WahoS8Context())
                {
                    _context.Entry<InventorySheetDetail>(inventoryDe).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        public static InventorySheet GetInventorySheetByID(int id) {
            try
            {
                using (var _context = new WahoS8Context())
                {
                    return _context.InventorySheets.Include(i => i.UserNameNavigation)
                        .FirstOrDefault(sb => sb.InventorySheetId == id);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static List<InventorySheetDetail> getInventorySheetDetailPaging(int pageIndex, int pageSize, int id)
        {
            try
            {
                using (var _context = new WahoS8Context())
                {
                    List<InventorySheetDetail> inventorySheetDetails = new List<InventorySheetDetail>();
                    inventorySheetDetails = _context.InventorySheetDetails
                                 .Include(i => i.InventorySheet)
                                 .Include(i => i.Product)
                                 .Include(i => i.InventorySheet.UserNameNavigation)
                                 .Where(i => i.InventorySheetId == id)
                                 .OrderBy(i => i.InventorySheetId)
                                 .Skip((pageIndex - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToList();
                    return inventorySheetDetails;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            
        }
        public static List<InventorySheetDetail> GetInventorySheetDetailByID(int inventory_id)
        {
            try
            {
                using (var _context = new WahoS8Context())
                {
                    return _context.InventorySheetDetails.Include(i => i.InventorySheet)
                                 .Include(i => i.Product)
                                 .Include(i => i.InventorySheet.UserNameNavigation)
                                 .Where(i => i.InventorySheetId == inventory_id).ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static int CountInventorySheetPaging(string textSearch,string employeeID, string raw_dateFrom, string raw_dateTo, int wahoId)
        {
            int count = 0;
            try
            {
                using (var _context = new WahoS8Context())
                {
                    var query = _context.InventorySheets.Include(p => p.UserNameNavigation)
                           .Where(i => i.Active == true);
                    if (!string.IsNullOrEmpty(textSearch))
                    {
                        query = query.Where(i => i.UserNameNavigation.EmployeeName.ToLower().Contains(textSearch)
                                   || i.Description.ToLower().Contains(textSearch));
                    }
                    // filter employee
                    if (employeeID != "all")
                    {
                        query = query.Where(i => i.UserName == employeeID);
                    }
                    // compare date to filter
                    if (!string.IsNullOrEmpty(raw_dateFrom))
                    {
                        if (!string.IsNullOrEmpty(raw_dateTo))
                        {
                            query = query.Where(i => i.Date >= DateTime.Parse(raw_dateFrom) && i.Date <= DateTime.Parse(raw_dateTo));
                        }
                        else
                        {
                            query = query.Where(i => i.Date >= DateTime.Parse(raw_dateFrom));
                        }

                    }
                    if (!string.IsNullOrEmpty(raw_dateTo))
                    {
                        query = query.Where(i => i.Date <= DateTime.Parse(raw_dateTo));
                    }
                    count = query.Where(i => i.WahoId == wahoId).Count();
                    return count;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static List<InventorySheet> GetInventorySheetsPaging(int pageIndex, int pageSize, string textSearch, string employeeID, string raw_dateFrom, string raw_dateTo, int wahoId)
        {
            try
            {
                using (var _context = new WahoS8Context())
                {
                    DateTime dateFrom = DateTime.Now;
                    DateTime dateTo = DateTime.Now;
                    
                    if (!string.IsNullOrEmpty(raw_dateFrom))
                    {
                        dateFrom = DateTime.Parse(raw_dateFrom);
                    }
                    else
                    {
                        raw_dateFrom = "";
                    }
                    if (!string.IsNullOrEmpty(raw_dateTo))
                    {
                        dateTo = DateTime.Parse(raw_dateTo);
                    }
                    else
                    {
                        raw_dateTo = "";
                    }
                    List<InventorySheet> inventories = new List<InventorySheet>();
                    var query = _context.InventorySheets.Include(i => i.UserNameNavigation)
                                                        .Where(i => i.Active == true);
                    if (!string.IsNullOrEmpty(textSearch))
                    {
                        query = query.Where(i => i.UserNameNavigation.EmployeeName.ToLower().Contains(textSearch.ToLower())
                                                                    || i.Description.ToLower().Contains(textSearch.ToLower()));
                    }
                    //filter date
                    if (!string.IsNullOrEmpty(raw_dateFrom))
                    {
                        if (!string.IsNullOrEmpty(raw_dateTo))
                        {
                            query = query.Where(i => i.Date >= dateFrom && i.Date <= dateTo);
                        }
                        else
                        {
                            query = query.Where(i => i.Date >= dateFrom);
                        }

                    }
                    if (!string.IsNullOrEmpty(raw_dateTo))
                    {
                        query = query.Where(i => i.Date <= dateTo);
                    }
                    // filter employee
                    if (employeeID != "all")
                    {
                        query = query.Where(i => i.UserName.Contains(employeeID));
                    }
                    inventories = query.Where(i => i.WahoId == wahoId)
                                 .OrderBy(i => i.InventorySheetId)
                                 .Skip((pageIndex - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToList();

                    return inventories;
                }
            } 
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
