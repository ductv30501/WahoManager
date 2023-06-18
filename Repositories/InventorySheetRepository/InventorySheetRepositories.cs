using BusinessObjects.WahoModels;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.InventorySheetViewModels;

namespace Repositories.InventorySheetRepository
{
    public class InventorySheetRepositories : IInventorySheetRepositories
    {
        public InventorySheet GetInventorySheetByID(int InventoryId) => InventorySheetDAO.GetInventorySheetByID(InventoryId);

        public int GetInventorySheetCount(string textSearch, string employeeID, string raw_dateFrom, string raw_dateTo, int wahoId) => InventorySheetDAO.CountInventorySheetPaging(textSearch,employeeID,raw_dateFrom,raw_dateTo,wahoId);

        public List<InventorySheetDetail> GetInventorySheetDetailByID(int inventory_id) => InventorySheetDAO.GetInventorySheetDetailByID(inventory_id);

        public InventorySheetDetail GetInventorySheetDetailByIDAndProId(int pro_id, int inventoryid) => InventorySheetDAO.GetInventorySheetDetailByIDAndProId(pro_id, inventoryid);

        public List<InventorySheetDetail> getInventorySheetDetailPaging(int pageIndex, int pageSize, int InventoryId) => InventorySheetDAO.getInventorySheetDetailPaging(pageIndex, pageSize,InventoryId);

        public List<InventorySheet> GetInventorySheetsInWaho(int wahoId) => InventorySheetDAO.GetInventorySheets(wahoId);

        public List<InventorySheet> inventorySheetsPaging(int pageIndex, int pageSize, string textSearch, string employeeID, string raw_dateFrom, string raw_dateTo, int wahoId) 
            => InventorySheetDAO.GetInventorySheetsPaging(pageIndex,pageSize,textSearch,employeeID,raw_dateFrom,raw_dateTo,wahoId);

        public void UpdateInventorySheet(InventorySheetVM inventoryVM) => InventorySheetDAO.UpdateInventorySheet(inventoryVM);

        public void UpdateInventorySheetDetail(InventorySheetDetailVM inventoryDetailVM) => InventorySheetDAO.UpdateInventorySheetDetail(inventoryDetailVM);
    }
}
