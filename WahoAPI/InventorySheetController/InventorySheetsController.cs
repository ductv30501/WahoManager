using BusinessObjects.WahoModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.InventorySheetRepository;
using Repositories.ProductRepository;
using ViewModels.InventorySheetViewModels;
using ViewModels.ProductViewModels;

namespace WahoAPI.InventorySheetController
{
    [Route("waho/[controller]")]
    [ApiController]
    public class InventorySheetsController : ControllerBase
    {
        private IInventorySheetRepositories respository = new InventorySheetRepositories();
        [HttpGet("inventories")]
        public ActionResult<List<InventorySheet>> getinventories(int wahoId)
        {
            List<InventorySheet> inventories = respository.GetInventorySheetsInWaho (wahoId);
            if (inventories == null)
            {
                return NotFound();
            }
            return Ok(inventories);
        }
        [HttpGet("countInventories")]
        public ActionResult<int> countInventories(string? textSearch, string employeeID, string? raw_dateFrom, string? raw_dateTo, int wahoId)
        {
            int total = respository.GetInventorySheetCount(textSearch, employeeID, raw_dateFrom, raw_dateTo, wahoId);
            if (total == 0)
            {
                return NotFound();
            }
            return Ok(total);
        }
        [HttpGet("getInventoryPaging")]
        public ActionResult<List<InventorySheet>> getInventoryPaging(int pageIndex, int pageSize, string? textSearch, string employeeID, string? raw_dateFrom, string? raw_dateTo, int wahoId)
        {
            List<InventorySheet> InventoryPaging = respository.inventorySheetsPaging(pageIndex, pageSize, textSearch, employeeID, raw_dateFrom, raw_dateTo, wahoId);
            if (InventoryPaging.Count == 0)
            {
                return NotFound();
            }
            return Ok(InventoryPaging);
        }
        [HttpGet("getInventorySheetById")]
        public ActionResult<InventorySheet> getInventorySheetById(int inventorySheetId)
        {
            InventorySheet Inventory = respository.GetInventorySheetByID(inventorySheetId);
            if (Inventory == null)
            {
                return NotFound();
            }
            return Ok(Inventory);
        }
        [HttpGet("getInventoryDetails")]
        public ActionResult<List<InventorySheetDetail>> getInventoryDetails(int inventorySheetId)
        {
            List<InventorySheetDetail> InventoryDetail = respository.GetInventorySheetDetailByID(inventorySheetId);
            if (InventoryDetail.Count == 0)
            {
                return NotFound();
            }
            return Ok(InventoryDetail);
        }
        [HttpGet("getInventoryDetailsPaging")]
        public ActionResult<List<InventorySheetDetail>> getInventoryDetailsPaging(int pageIndex, int pageSize, int InventoryId)
        {
            List<InventorySheetDetail> InventoryDetail = respository.getInventorySheetDetailPaging(pageIndex, pageSize, InventoryId);
            if (InventoryDetail.Count == 0)
            {
                return NotFound();
            }
            return Ok(InventoryDetail);
        }
        [HttpGet("getInventoryDetails-ByProId-InvenId")]
        public ActionResult<InventorySheetDetail> getInventoryDetailsByProIdInvenId(int productId,int inventorySheetId)
        {
            InventorySheetDetail InventoryDetail = respository.GetInventorySheetDetailByIDAndProId(productId,inventorySheetId);
            if (InventoryDetail == null)
            {
                return NotFound();
            }
            return Ok(InventoryDetail);
        }
        [HttpPut]
        public IActionResult PutInventory(InventorySheetVM Ivm)
        {
            respository.UpdateInventorySheet(Ivm);
            return Ok();
        }
        [HttpPut("detail")]
        public IActionResult PutInventoryDetail(InventorySheetDetailVM IDetailvm)
        {
            respository.UpdateInventorySheetDetail(IDetailvm);
            return Ok();
        }
    }
}
