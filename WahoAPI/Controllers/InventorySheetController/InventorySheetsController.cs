using BusinessObjects.WahoModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.InventorySheetRepository;
using Repositories.ProductRepository;
using ViewModels.InventorySheetViewModels;
using ViewModels.ProductViewModels;

namespace WahoAPI.Controllers.InventorySheetController
{
    [Route("waho/[controller]")]
    [ApiController]

    public class InventorySheetsController : ControllerBase
    {
        private IInventorySheetRepositories respository = new InventorySheetRepositories();
        [HttpGet("inventories")]
        [Authorize]

        public ActionResult<List<InventorySheet>> getinventories(int wahoId)
        {
            List<InventorySheet> inventories = respository.GetInventorySheetsInWaho(wahoId);
            if (inventories == null)
            {
                return NotFound();
            }
            return Ok(inventories);
        }
        [HttpGet("countInventories")]
        [Authorize]

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
        [Authorize]

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
        [Authorize]

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
        [Authorize]

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
        [Authorize]

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
        [Authorize]

        public ActionResult<InventorySheetDetail> getInventoryDetailsByProIdInvenId(int productId, int inventorySheetId)
        {
            InventorySheetDetail InventoryDetail = respository.GetInventorySheetDetailByIDAndProId(productId, inventorySheetId);
            if (InventoryDetail == null)
            {
                return NotFound();
            }
            return Ok(InventoryDetail);
        }
        [HttpPut]
        [Authorize]

        public IActionResult PutInventory(InventorySheetVM Ivm)
        {
            respository.UpdateInventorySheet(Ivm);
            return Ok();
        }
        [HttpPost]
        [Authorize]

        public IActionResult PosttInventory(InventorySheetVM Ivm)
        {
            int inventoryId = respository.SaveInventorySheet(Ivm);
            PostInventorySheetVM postInventorySheetVM = new PostInventorySheetVM();
            postInventorySheetVM.InventorySheetId = inventoryId;
            return Ok(postInventorySheetVM);
        }
        [HttpPost("detail")]
        [Authorize]

        public IActionResult PosttInventoryDetail([FromBody] List<InventorySheetDetailVM> IDevm)
        {
            respository.saveInventoryDetail(IDevm);
            return Ok();
        }
        [HttpPut("detail")]
        [Authorize]

        public IActionResult PutInventoryDetail(InventorySheetDetailVM IDetailvm)
        {
            respository.UpdateInventorySheetDetail(IDetailvm);
            return Ok();
        }
    }
}
