using BusinessObjects.WahoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.InventorySheetViewModels;

namespace Repositories.InventorySheetRepository
{
    public interface IInventorySheetRepositories
    {
        List<InventorySheet> GetInventorySheetsInWaho(int wahoId);
        int GetInventorySheetCount(string textSearch, string employeeID, string raw_dateFrom, string raw_dateTo, int wahoId);
        List<InventorySheet> inventorySheetsPaging(int pageIndex, int pageSize, string textSearch, string employeeID, string raw_dateFrom, string raw_dateTo, int wahoId);
        InventorySheet GetInventorySheetByID(int InventoryId);
        List<InventorySheetDetail> getInventorySheetDetailPaging(int pageIndex, int pageSize, int InventoryId);
        List<InventorySheetDetail> GetInventorySheetDetailByID(int inventory_id);
        InventorySheetDetail GetInventorySheetDetailByIDAndProId(int pro_id, int inventoryid);
        void UpdateInventorySheet(InventorySheetVM inventoryVM);
        void UpdateInventorySheetDetail(InventorySheetDetailVM inventoryDetailVM);
        int SaveInventorySheet(InventorySheetVM inventoryVM);
        void saveInventoryDetail(List<InventorySheetDetailVM> InDeVm);
    }
}
